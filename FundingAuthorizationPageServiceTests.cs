using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Enerbank.FundingManagement.Api.Constants;
using Enerbank.FundingManagement.Api.Domain.Services.FundingAuthorizationPageService;
using Enerbank.FundingManagement.Api.Domain.Services.FundingAuthorizationPageService.Models;
using EnerBank.FundingManagement.AchApi.Models.Request;
using EnerBank.FundingManagement.LoanServicingClient.Models.Responses;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Enerbank.FundingManagement.Api.Tests.Services {
    /// <summary>
    /// Tests the Funding Authorization Page Service
    /// </summary>
    public class FundingAuthorizationPageServiceTests : IClassFixture<FundingAuthorizationPageServiceTestFixture> {
        /// <summary>
        /// The test fixture to be shared amongst these tests.
        /// </summary>
        private FundingAuthorizationPageServiceTestFixture Fixture { get; set; }

        private FundingAuthorizationPageService Service { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingAuthorizationPageServiceTests"/> class.
        /// </summary>
        /// <param name="fixture">The test fixture</param>
        public FundingAuthorizationPageServiceTests(FundingAuthorizationPageServiceTestFixture fixture) {
            Fixture = fixture;
            Service = new FundingAuthorizationPageService(fixture.serviceProviderMock.Object,
                fixture.loanServicingMock.Object,
                new NullLogger<FundingAuthorizationPageService>(),
                fixture.achApiClientMocks.achApiClientMock.Object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        public async Task ManualFundingFlagSet(bool inDatabase, bool useAchApi) {
            // arrange
            var target = Fixture.Db.Loans.Skip(25).FirstOrDefault();
            Fixture.achApiClientMocks.achApiClientMock.Reset();

            if (!inDatabase) {
                Fixture.loanServicingMock.Setup(x => x.GetManualFundingLoansAsync()).Returns(Task.FromResult(new List<LoanResponseModel> {
                    new LoanResponseModel {
                        LoanId = target.Id,
                        ContractorId = target.Contractor.Id,
                    }
                }));
            }

            dynamic filter = new ExpandoObject();
            filter.TextSearch = target.Id.ToString();
            var request = await Fixture.Db.FundingRequests.FirstOrDefaultAsync(r => r.LoanId == target.Id).ConfigureAwait(false);

            await Fixture.Db.SaveChangesAsync().ConfigureAwait(false);

            if (useAchApi) {
                Fixture.achApiClientMocks.achApiClientMock.SetupGet(a => a.UseAchApi).Returns(true);
                Fixture.achApiClientMocks.SetupSearchContractor(target.Contractor.Id, false, null, true);
            } else {
                target.ManualFunding = true;
            }

            // act
            FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);

            // assert
            authorizations.Summaries.FirstOrDefault(x => x.AuthorizationNumber == request.Id).Tags.Should().Contain(FMSConstants.LoanExceptionTags.ManualFunding);

            if (useAchApi) {
                Fixture.achApiClientMocks.achApiClientMock.Verify(a => a.SearchContractorsAsync(It.Is<ContractorSearchRequestModel>(c => c.ContractorId == target.Contractor.Id)), Times.Once);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FrozenFlagSet() {
            var target = Fixture.Db.Loans.Skip(26).FirstOrDefault();

            dynamic filter = new ExpandoObject();
            filter.TextSearch = target.Id.ToString();
            var request = await Fixture.Db.FundingRequests.FirstOrDefaultAsync(r => r.LoanId == target.Id).ConfigureAwait(false);
            target.IsFrozen = true;
            await Fixture.Db.SaveChangesAsync().ConfigureAwait(false);

            FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);

            authorizations.Summaries.FirstOrDefault(x => x.AuthorizationNumber == request.Id).Tags.Should().Contain(FMSConstants.LoanExceptionTags.Frozen);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public async Task NachaFlagSet(bool requiresPrenote, bool prenoteExpired, bool useAchApi) {
            // arrange
            var target = Fixture.Db.Loans.Skip(27).FirstOrDefault();

            dynamic filter = new ExpandoObject();
            filter.TextSearch = target.Id.ToString();
            var request = await Fixture.Db.FundingRequests.FirstOrDefaultAsync(r => r.LoanId == target.Id).ConfigureAwait(false);

            await Fixture.Db.SaveChangesAsync().ConfigureAwait(false);

            if (useAchApi) {
                Fixture.achApiClientMocks.achApiClientMock.SetupGet(a => a.UseAchApi).Returns(true);
                Fixture.achApiClientMocks.SetupSearchContractor(target.Contractor.Id, true, null, false);
            } else {
                target.Contractor.RequiresPrenote = requiresPrenote;
                target.Contractor.PrenoteValidationExpiration = prenoteExpired ? DateTimeOffset.UtcNow.AddDays(1) : DateTimeOffset.UtcNow.AddDays(-1);
            }

            // act
            FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);

            // assert
            authorizations.Summaries.FirstOrDefault(x => x.AuthorizationNumber == request.Id).Tags.Should().Contain(FMSConstants.LoanExceptionTags.NachaWait);

            if (useAchApi) {
                Fixture.achApiClientMocks.achApiClientMock.Verify(a => a.SearchContractorsAsync(It.Is<ContractorSearchRequestModel>(c => c.ContractorId == target.Contractor.Id)), Times.AtLeastOnce);
            }
        }

        /// <summary>
        /// Tests that no filter returns complete, unfiltered results.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithNoFilterObject_ReturnsAll() {
            FundingAuthorizationPageResult result = await Service.GetFundingAuthorizatonPageAsync(1, 10);
            Assert.Equal(10, result.Summaries.Count());
            Assert.Equal(600, result.TotalCount);
        }

        /// <summary>
        /// Tests that a valid filter is applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithValidFilter_ReturnsExpectedResult() {
            var target = Fixture.Db.Loans.Skip(123).FirstOrDefault();

            dynamic filter = new ExpandoObject();
            filter.ApplicantFirstName = target.Applicant.FirstName;
            filter.ApplicantLastName = target.Applicant.LastName;
            filter.ApplicationNumber = target.ApplicationNumber;
            filter.Status = "pending";

            FundingAuthorizationPageResult result = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);
            // Should return the 2 of 3 funding requests.
            Assert.Equal(2, result.TotalCount);

            foreach (var authorization in result.Summaries) {
                Assert.Equal(target.Applicant.FirstName, authorization.ApplicantFirstName);
                Assert.Equal(target.Applicant.LastName, authorization.ApplicantLastName);
                Assert.Equal(target.ApplicationNumber, authorization.ApplicationNumber);
            }
        }

        /// <summary>
        /// Tests that a valid filter is applied, working when a collection of string values is provided by making a case-insensitive search.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithValidFilterWithCollectionOfStringValues_ReturnsExpectedResultCaseInsensitive() {
            var target = Fixture.Db.Loans;
            var testStatuses = new List<string>() { "PeNdinG", "abc123" };

            dynamic filter = new ExpandoObject();
            filter.Status = testStatuses;

            FundingAuthorizationPageResult result = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);
            foreach (var authorization in result.Summaries) {
                var isValid = testStatuses.Any(status => status.Equals(authorization.Status, StringComparison.CurrentCultureIgnoreCase));

                Assert.True(isValid);
            }
        }

        /// <summary>
        /// Tests that a valid filter is applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithCapitalizedFilter_ReturnsExpectedResult() {
            var target = Fixture.Db.Loans.Skip(123).FirstOrDefault();

            dynamic filter = new ExpandoObject();
            filter.APPLICANTFirstName = target.Applicant.FirstName;
            filter.ApplicantLASTNAME = target.Applicant.LastName;
            filter.ApplicaTIONNumber = target.ApplicationNumber;
            filter.StAtUs = "pending";

            FundingAuthorizationPageResult result = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);
            // Should return the 2 of 3 funding requests.
            Assert.Equal(2, result.TotalCount);

            foreach (var authorization in result.Summaries) {
                Assert.Equal(target.Applicant.FirstName, authorization.ApplicantFirstName);
                Assert.Equal(target.Applicant.LastName, authorization.ApplicantLastName);
                Assert.Equal(target.ApplicationNumber, authorization.ApplicationNumber);
            }
        }

        /// <summary>
        /// Tests that an invalid filter doesnt throw an error, and is not applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithInvalidFilter_ThrowsError() {
            var target = Fixture.Db.Loans.Skip(123).FirstOrDefault();

            dynamic filter = new ExpandoObject();
            filter.TypeOfPizza = "Hawaiian";
            await Assert.ThrowsAsync<Exceptions.BadRequestMessage>(() => Service.GetFundingAuthorizatonPageAsync(1, 10, filter));
        }

        /// <summary>
        /// Tests that TextSearch returns valid results.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithValidSearch_ReturnsExpectedResult() {
            var target = Fixture.Db.Loans.Skip(123).FirstOrDefault();

            dynamic filter = new ExpandoObject();
            filter.TextSearch = target.ApplicationNumber.Substring(4);

            FundingAuthorizationPageResult result = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);
            // Should be the 3 funding requests generated per loan.
            Assert.Equal(3, result.TotalCount);

            foreach (var authorization in result.Summaries)
                Assert.Contains(target.ApplicationNumber.Substring(4), authorization.ApplicationNumber);
        }

        /// <summary>
        /// Tests that TextSearch returns valid results against a Guid
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithValidSearchOfAnId_ReturnsExpectedResult() {
            var target = Fixture.Db.Loans.Skip(123).FirstOrDefault();

            dynamic filter = new ExpandoObject();
            filter.TextSearch = target.Id.ToString().Substring(4);

            FundingAuthorizationPageResult result = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);
            // Should be the 3 funding requests generated per loan.
            Assert.Equal(3, result.TotalCount);

            foreach (var authorization in result.Summaries)
                Assert.Contains(target.Id.ToString().Substring(4), authorization.LoanId.ToString());
        }

        /// <summary>
        /// Tests that passing a page number larger than the dataset simply returns empty data.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithPageNumberLargerThanDataset_ReturnsNoData() {
            // There are 600 authorizations.
            FundingAuthorizationPageResult result = await Service.GetFundingAuthorizatonPageAsync(61, 10);
            Assert.Empty(result.Summaries);
            Assert.Equal(600, result.TotalCount);
        }

        /// <summary>
        /// Tests that the results between one page and another are exclusive.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_Pages_ReturnUniqueResults() {
            var usedIds = new HashSet<Guid>();

            dynamic filter = new ExpandoObject();
            filter.Status = "hold";

            string sort = "ApplicationNumber";

            for (int i = 1; i <= 10; i++) {
                FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(i, 10, filter, sort);
                Assert.Equal(200, authorizations.TotalCount);
                foreach (var authorization in authorizations.Summaries) {
                    Assert.DoesNotContain(authorization.AuthorizationNumber, usedIds);

                    usedIds.Add(authorization.AuthorizationNumber);
                }
            }
        }

        /// <summary>
        /// Tests that a valid sort is applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithValidSort_ReturnsExpectedResult() {
            FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(1, 10, null, "ApplicantFirstName");
            var resultList = authorizations.Summaries.ToArray();
            var sortedList = authorizations.Summaries.OrderBy(x => x.ApplicantFirstName).ToArray();

            Assert.Equal(sortedList.Length, resultList.Length);
            for (int i = 0; i < resultList.Length; i++) {
                Assert.Equal(sortedList[i].ApplicantFirstName, resultList[i].ApplicantFirstName);
            }
        }

        /// <summary>
        /// Tests that a valid sort is applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_ApprovedRequestsFailWhenNotCompleted() {
            var target = Fixture.Db.Loans.Skip(99).FirstOrDefault();

            dynamic filter = new ExpandoObject();
            filter.TextSearch = target.Id.ToString();
            var request = await Fixture.Db.FundingRequests.FirstOrDefaultAsync(r => r.LoanId == target.Id).ConfigureAwait(false);
            var ach = Fixture.Db.ACHRequests.FirstOrDefault();
            ach.ApprovedBefore = DateTime.Now;
            request.ApprovedAt = ach.ApprovedBefore.AddHours(-1);
            request.Status = "approved";
            await Fixture.Db.SaveChangesAsync().ConfigureAwait(false);

            FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(1, 10, filter);

            authorizations.Summaries.FirstOrDefault(x => x.AuthorizationNumber == request.Id).FailedToFund.Should().BeTrue();
        }

        /// <summary>
        /// Tests that a valid ascending sort is applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithValidAscendingSort_ReturnsExpectedResult() {
            FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(1, 10, null, "ApplicantFirstName", FMSConstants.Paging.Ascending);
            var resultList = authorizations.Summaries.ToArray();
            var sortedList = Fixture.Db.FundingAuthorizationSummaries.OrderBy(x => x.ApplicantFirstName).ToArray();

            for (int i = 0; i < resultList.Length; i++) {
                Assert.Equal(sortedList[i].ApplicantFirstName, resultList[i].ApplicantFirstName);
            }
        }

        /// <summary>
        /// Tests that a valid descending sort is applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithValidDescendingSort_ReturnsExpectedResult() {
            FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(1, 10, null, "ApplicantFirstName", FMSConstants.Paging.Descending);
            var resultList = authorizations.Summaries.ToArray();
            var sortedList = Fixture.Db.FundingAuthorizationSummaries.OrderByDescending(x => x.ApplicantFirstName).ToArray();

            for (int i = 0; i < resultList.Length; i++) {
                Assert.Equal(sortedList[i].ApplicantFirstName, resultList[i].ApplicantFirstName);
            }
        }

        /// <summary>
        /// Tests that a valid sort is applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithInvalidSort_ThrowsError() {
            dynamic filter = new ExpandoObject();
            await Assert.ThrowsAsync<System.Linq.Dynamic.Core.Exceptions.ParseException>(() => Service.GetFundingAuthorizatonPageAsync(1, 10, filter, "cats"));
        }

        /// <summary>
        /// Tests that an invalid page aparameter returns a bad request.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithInvalidPageParam_ThrowsError() {
            dynamic filter = new ExpandoObject();
            await Assert.ThrowsAsync<Exceptions.BadRequestMessage>(() => Service.GetFundingAuthorizatonPageAsync(0, 10, filter, "cats"));
        }

        /// <summary>
        /// Tests that invalid page size returns bad request.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithInvalidPageSize_ThrowsError() {
            dynamic filter = new ExpandoObject();
            await Assert.ThrowsAsync<Exceptions.BadRequestMessage>(() => Service.GetFundingAuthorizatonPageAsync(1, 0, filter, "cats"));
        }

        /// <summary>
        /// Tests that a valid sort is applied.
        /// </summary>
        [Fact]
        [Trait(Constants.TestType.Category, Constants.TestCategories.UnitTest)]
        public async Task GetFundingAuthorizatonPage_WithCapitalizedSort_ReturnsExpectedResult() {
            FundingAuthorizationPageResult authorizations = await Service.GetFundingAuthorizatonPageAsync(1, 10, null, "sTAtus");
            var resultList = authorizations.Summaries.ToArray();
            var sortedList = authorizations.Summaries.OrderBy(x => x.Status).ToArray();

            Assert.Equal(sortedList.Length, resultList.Length);
            for (int i = 0; i < resultList.Length; i++) {
                Assert.Equal(sortedList[i].Status, resultList[i].Status);
            }

        }

        public const string SEARCH_PREDICATE = "(AuthorizationNumber.ToString().Contains(\"1234\") OR ApplicationNumber.Contains(\"1234\") OR SpectrumAccountNumber.Contains(\"1234\") OR LoanId.ToString().Contains(\"1234\") OR Status.Contains(\"1234\") OR StatusDetail.Contains(\"1234\") OR ApplicantFirstName.Contains(\"1234\") OR ApplicantLastName.Contains(\"1234\") OR ApplicantState.Contains(\"1234\") OR CommunicationMethod.Contains(\"1234\") OR CommunicationPhoneNumber.Contains(\"1234\") OR Contractor.Contains(\"1234\") OR ContractorState.Contains(\"1234\") OR AccountStatus.Contains(\"1234\") OR ClosedToPostingCode.Contains(\"1234\") OR FundingNoticeMethod.Contains(\"1234\"))";
        [Fact]
        public void ShouldGetSearchPredicate() {
            // arrange
            dynamic filter = new ExpandoObject();
            filter.textSearch = "1234";

            // act
            var s = FundingAuthorizationPageService.GetFilterPredicate(filter);

            // assert
            Assert.NotNull(s);
            Assert.Equal(SEARCH_PREDICATE, s);
        }

        public const string FILTER_PREDICATE = "(applicantFirstName = \"1234\") AND (applicantLastName = \"abc\")";
        [Fact]
        public void ShouldGetFilterPredicate() {
            // arrange
            dynamic filter = new ExpandoObject();
            filter.applicantFirstName = "1234";
            filter.applicantLastName = "abc";

            // act
            var s = FundingAuthorizationPageService.GetFilterPredicate(filter);

            // assert
            Assert.NotNull(s);
            Assert.Equal(FILTER_PREDICATE, s);
        }

        public const string FILTERCOLLECTION_PREDICATE = "(status in (\"one\",\"two\"))";
        [Fact]
        public void ShouldGetFilterCollectionPredicate() {
            // arrange
            dynamic filter = new ExpandoObject();
            filter.status = new List<string>() { "one", "two" };

            // act
            var s = FundingAuthorizationPageService.GetFilterPredicate(filter);

            // assert
            Assert.NotNull(s);
            Assert.Equal(FILTERCOLLECTION_PREDICATE, s);
        }

        [Fact]
        public void ShouldGetAllPredicates() {
            // arrange
            dynamic filter = new ExpandoObject();
            filter.textSearch = "1234";
            filter.applicantFirstName = "1234";
            filter.applicantLastName = "abc";
            filter.status = new List<string>() { "one", "two" };

            // act
            var s = FundingAuthorizationPageService.GetFilterPredicate(filter);

            // assert
            Assert.NotNull(s);
            Assert.Equal($"{SEARCH_PREDICATE} AND {FILTER_PREDICATE} AND {FILTERCOLLECTION_PREDICATE}", s);
        }
    }
}
