using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Enerbank.FundingManagement.Api.Constants;
using Enerbank.FundingManagement.Api.Domain.Services.FundingAuthorizationPageService.Models;
using Enerbank.FundingManagement.Api.Exceptions;
using EnerBank.FundingManagement.AchApi;
using EnerBank.FundingManagement.AchApi.Models.Request;
using EnerBank.FundingManagement.AchApi.Models.Response;
using EnerBank.FundingManagement.Data.Database.Context;
using EnerBank.FundingManagement.Data.Database.Tables;
using EnerBank.FundingManagement.Data.Database.Views;
using EnerBank.FundingManagement.Data.Enumerations;
using EnerBank.FundingManagement.Data.Tables;
using EnerBank.FundingManagement.LoanServicingClient;
using EnerBank.FundingManagement.LoanServicingClient.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enerbank.FundingManagement.Api.Domain.Services.FundingAuthorizationPageService {
    /// <summary>
    /// The Paging Service for fully qualified Funding Authorizations.
    /// </summary>
    public class FundingAuthorizationPageService : IFundingAuthorizationPageService {
        /// <summary>
        /// Constant of the textSearch field name.
        /// </summary>
        public const string TextSearchFieldName = "textSearch";

        private readonly IServiceProvider serviceProvider;
        private readonly ILoanServicingClient loanServicingClient;
        private readonly ILogger<FundingAuthorizationPageService> logger;
        private readonly IAchApiClient achApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundingAuthorizationPageService"/> class.
        /// </summary>
        /// <param name="serviceProvider">Service Provider</param>
        /// <param name="loanServicingClient"></param>
        /// <param name="logger"></param>
        /// <param name="achApiClient"></param>
        public FundingAuthorizationPageService(IServiceProvider serviceProvider,
            ILoanServicingClient loanServicingClient,
            ILogger<FundingAuthorizationPageService> logger,
            IAchApiClient achApiClient) {
            this.serviceProvider = serviceProvider;
            this.loanServicingClient = loanServicingClient;
            this.logger = logger;
            this.achApiClient = achApiClient;
        }

        /// <summary>
        /// Grabs a page of Funding Authorizations. Currently uses a very 'loose' filter object.
        /// </summary>
        /// <param name="page">The page number</param>
        /// <param name="pageSize">The size of the page</param>
        /// <param name="filterByRequest">The (optional) filtering request</param>
        /// <param name="sortByRequest">The (optional) sorting request</param>
        /// <param name="sortDirectionRequest">The sort direction</param>
        /// <returns>A collection of FundingAuthorizations matching the criteria.</returns>
        public async Task<FundingAuthorizationPageResult> GetFundingAuthorizatonPageAsync(
            int page,
            int pageSize,
            ExpandoObject filterByRequest = null,
            string sortByRequest = null,
            string sortDirectionRequest = FMSConstants.Paging.Ascending) {
            using (var scope = serviceProvider.CreateScope()) {
                if (page <= 0 || pageSize <= 0) {
                    throw new BadRequestMessage();
                }

                if (sortDirectionRequest != null && !sortDirectionRequest.Equals(FMSConstants.Paging.Ascending, StringComparison.InvariantCultureIgnoreCase) && !sortDirectionRequest.Equals(FMSConstants.Paging.Descending, StringComparison.InvariantCultureIgnoreCase)) {
                    throw new BadRequestMessage();
                }

                var filter = GetFilterPredicate(filterByRequest);
                if (string.IsNullOrEmpty(filter)) {
                    filter = "(1 = 1)";
                }

                if (string.IsNullOrEmpty(sortByRequest)) {
                    sortByRequest = "CreatedDate";
                    sortDirectionRequest = "desc";
                }

                List<LoanResponseModel> manualLoans = await loanServicingClient.GetManualFundingLoansAsync().ConfigureAwait(false);

                var dbScope = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
                var countResult = await dbScope.FundingAuthorizationSummaries.Where(filter).CountAsync();

                IEnumerable<FundingAuthorizationSummary> summariesResult = await dbScope.FundingAuthorizationSummaries
                    .Where(filter)
                    .OrderBy($"{sortByRequest} {sortDirectionRequest}")
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                List<FundingAuthorizationSummary> summariesWithTags = new List<FundingAuthorizationSummary>();
                var contractorList = new List<Contractor>();

                foreach (var fr in summariesResult) {
                    if (fr.Tags == null) {
                        fr.Tags = new string[0];
                    }

                    var loan = await dbScope.Loans.FirstOrDefaultAsync(c => c.Id == fr.LoanId).ConfigureAwait(false);

                    var loanMatch = manualLoans.FirstOrDefault(x => x.LoanId == loan.Id);
                    if (loanMatch != null) {
                        loan.ManualFunding = true;
                    }
                    var contractorId = loan.ContractorId;

                    Contractor contractor = null;
                    List<ContractorResponseModel> achApiContractorResponse = null;

                    if (achApiClient.UseAchApi) {
                        if (!contractorList.Any(c => c.Id == contractorId)) {
                            achApiContractorResponse = await achApiClient.SearchContractorsAsync(new ContractorSearchRequestModel { ContractorId = contractorId }).ConfigureAwait(false);
                            contractor = MapToContractorEntity(achApiContractorResponse.First());
                            contractorList.Add(contractor);
                        } else {
                            contractor = contractorList.FirstOrDefault(c => c.Id == contractorId);
                        }
                    } else {
                        contractor = await dbScope.Contractors.FirstOrDefaultAsync(x => x.Id == contractorId).ConfigureAwait(false);
                    }

                    if (loan.IsFrozen) {
                        var tagsList = fr.Tags.ToList();
                        fr.Tags = tagsList.Append(FMSConstants.LoanExceptionTags.Frozen);
                    }
                    if (contractor.RequiresPrenote || contractor.PrenoteValidationExpiration > DateTimeOffset.UtcNow) {
                        var tagsList = fr.Tags.ToList();
                        fr.Tags = tagsList.Append(FMSConstants.LoanExceptionTags.NachaWait);
                    }

                    if (loan.ManualFunding || (achApiClient.UseAchApi && achApiContractorResponse != null && achApiContractorResponse.Any() && achApiContractorResponse.First().ManualFunding)) {
                        var tagsList = fr.Tags.ToList();
                        fr.Tags = tagsList.Append(FMSConstants.LoanExceptionTags.ManualFunding);
                    }
                    fr.ClosedToPostingCode = loan.ClosedToPostingCode;
                    fr.AccountStatus = loan.AccountStatus;
                    summariesWithTags.Add(fr);
                }

                List<FundingAuthorizationSummary> summariesResponse = await GetFundingAuthorizationSummaryAsync(dbScope, summariesWithTags);

                return new FundingAuthorizationPageResult() {
                    TotalCount = countResult,
                    Summaries = summariesResponse
                };
            }
        }

        private Contractor MapToContractorEntity(ContractorResponseModel contractorResponseModel) {
            return new Contractor {
                Id = contractorResponseModel.ContractorId,
                Name = contractorResponseModel.Name,
                RequiresPrenote = contractorResponseModel.RequiresPrenote,
                PrenoteValidationExpiration = contractorResponseModel.PrenoteValidationExpiration
            };
        }

        private async Task<List<FundingAuthorizationSummary>> GetFundingAuthorizationSummaryAsync(MasterDbContext dbScope, IEnumerable<FundingAuthorizationSummary> summariesResult) {
            List<FundingAuthorizationSummary> summariesResponse = new List<FundingAuthorizationSummary>();
            ACHFile achFile = await dbScope.ACHFiles
                .Where(a => a.Status == ACHFileStatusType.Completed)
                .OrderByDescending(a => a.GenerationCompletedAt)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            if (achFile != null) {
                ACHRequest achRequest = await dbScope.ACHRequests.FirstOrDefaultAsync(a => a.ACHFile == achFile).ConfigureAwait(false);
                if (achRequest != null) {
                    foreach (var fr in summariesResult) {
                        if (fr.ApprovedAt != default && fr.Status.ToLower() == "approved" && fr.ApprovedAt < achRequest.ApprovedBefore) {
                            fr.FailedToFund = true;
                        }
                        summariesResponse.Add(fr);
                    }
                }
            }
            return summariesResponse;
        }

        public static string GetFilterPredicate(ExpandoObject filterByRequest) {
            // If the filter is null, return a predicate that passes all info.
            if (filterByRequest == null) {
                return null;
            }

            IDictionary<string, object> filterDictionary = filterByRequest;

            // Verify the properties are real and that the predicate will work.
            // Errors can't be thrown within the predicate, because they won't be exposed until the IEnumerable is
            // actually iterated over.
            foreach (var key in filterDictionary.Keys) {
                if (!key.Equals(TextSearchFieldName, StringComparison.CurrentCultureIgnoreCase)
                    && !typeof(FundingAuthorizationSummary).GetProperties().Any(x => string.Equals(x.Name, key, StringComparison.CurrentCultureIgnoreCase)))
                    throw new BadRequestMessage();
            }

            var exp = new List<string>();
            foreach (var entry in filterDictionary) {
                if (entry.Key.Equals(TextSearchFieldName, StringComparison.CurrentCultureIgnoreCase))
                    exp.Add(OnSearch(entry.Value));
                else if (entry.Value.GetType().GetInterfaces().Any(type => type == typeof(ICollection))) {
                    exp.Add(OnFilterCollection(entry.Key, entry.Value));
                } else {
                    exp.Add(OnFilter(entry.Key, entry.Value));
                }
            }

            var x = exp.Select(s => "(" + s + ")");
            var e = string.Join(" AND ", x.ToArray());
            return e;
        }

        private static string OnSearch(dynamic value) {
            var searchablePropertyTypes = new HashSet<Type>() {
                typeof(string),
                typeof(Guid)
            };

            var properties = typeof(FundingAuthorizationSummary)
                .GetProperties()
                .Where(x => searchablePropertyTypes.Contains(x.PropertyType))
                .Select(x => $"{x.Name}{(x.PropertyType == typeof(Guid) ? ".ToString()" : "")}.Contains(\"{value}\")")
                .ToList();

            return string.Join(" OR ", properties.ToArray());
        }

        private static string OnFilter(string propertyName, dynamic value) {
            var s = $"{propertyName} = \"{value}\"";
            return s;
        }

        private static string OnFilterCollection(string propertyName, dynamic values) {
            var s = $"{propertyName} in (";
            var ss = "";
            foreach (var value in values) {
                ss += $"\"{value}\",";
            }
            ss = ss.Substring(0, ss.Length - 1);

            s += $"{ss})";
            return s;
        }
    }
}
