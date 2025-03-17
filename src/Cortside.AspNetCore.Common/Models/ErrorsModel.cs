using System;
using System.Collections.Generic;
using Cortside.Common.Messages;

namespace Cortside.AspNetCore.Common.Models {
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
        public ErrorsModel(IList<ErrorModel> errors) {
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
        public IList<ErrorModel> Errors { get; set; }

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

        /// <summary>
        /// Adds error model
        /// </summary>
        /// <param name="message"></param>
        public void AddError(string type, string property, string message, Exception exception = null) {
            var error = new ErrorModel() {
                Type = type,
                Property = property,
                Message = message
            };

            Errors.Add(error);
        }
    }
}
