using System;
using System.Collections.Generic;
using Cortside.Common.Messages;

namespace Cortside.AspNetCore.Filters.Models {
    /// <summary>
    /// Errors Model
    /// </summary>
    public class ErrorsModel {
        /// <summary>
        /// Errors model constructor
        /// </summary>
        public ErrorsModel() {
            Errors = new List<ErrorModel>();
        }

        /// <summary>
        /// Errors model constructor
        /// </summary>
        public ErrorsModel(List<ErrorModel> errors) {
            Errors = errors ?? new List<ErrorModel>();
        }

        /// <summary>
        /// Create new instance from an exception
        /// </summary>
        /// <param name="ex"></param>
        public ErrorsModel(Exception ex) {
            Errors = new List<ErrorModel>() { new ErrorModel(ex) };
        }

        /// <summary>
        /// Errors List
        /// </summary>
        public List<ErrorModel> Errors { get; set; }

        /// <summary>
        /// Adds error model
        /// </summary>
        /// <param name="message"></param>
        public void AddError(MessageException message) {
            var error = new ErrorModel() {
                Type = message.GetType().Name,
                Property = message.Property,
                Message = message.Message
            };

            Errors.Add(error);
        }
    }
}
