using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EBOA.DomainService.Search.Interfaces;
using EBOA.Exceptions.Core;
using EBOA.Exceptions.ValidationMessages;

namespace EBOA.DomainService.Search {
    public class SortParamValidator : ISortParamValidator {
        public ValidationMessageList Validate<T>(string sortParams) {
            return Validate<T>(null, sortParams);
        }

        public ValidationMessageList Validate<T>(ISortParamMapper<T> adapater, string sortParams) {
            ValidationMessageList messages = new ValidationMessageList();
            if (string.IsNullOrEmpty(sortParams)) {
                return messages;
            }

            var codes = new List<string>();
            const BindingFlags flags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            foreach (var sortParameter in sortParams.Split(',')) {
                string propertyName = sortParameter;
                string orderType = sortParameter[..1];

                if (orderType == "-") {
                    propertyName = sortParameter[1..];
                }

                var propertyType = typeof(T);
                if (adapater != null) {
                    propertyName = adapater.MapProperty(propertyName);
                }

                if (propertyName.Contains('.')) {
                    foreach (var member in propertyName.Split('.')) {
                        var propertyInfo = propertyType.GetProperty(member, flags);
                        if (propertyInfo == null) {
                            codes.Add($"SortProperty{member}Invalid");
                            break;
                        }

                        propertyType = propertyInfo.PropertyType;
                        if (propertyType.IsGenericType) {
                            propertyType = propertyType.GetGenericArguments().FirstOrDefault();
                            if (propertyType == null) {
                                codes.Add($"SortProperty{member}Invalid");
                                break;
                            }
                        }
                    }
                } else if (!string.IsNullOrEmpty(propertyName)) {
                    var propertyInfo = propertyType.GetProperty(propertyName, flags);
                    if (propertyInfo == null) {
                        codes.Add($"SortProperty{propertyName}Invalid");
                    }
                }
            }

            if (codes.Count > 0) {
                messages.AddValidationMessage(() => new InvalidValueMessage("sort", sortParams));
            }

            return messages;
        }
    }
}
