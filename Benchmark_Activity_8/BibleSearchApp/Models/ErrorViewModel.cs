/*!
 * BibleSearchApp
 * 
 * File: ErrorViewModel.cs
 * Description: Represents the model used to display error information in the BibleSearchApp.
 *              Contains details about the request that caused the error, facilitating debugging and user feedback.
 * Author: Alex Frear
 * Created: 2024-04-27
 * License: MIT License
 */

namespace BibleSearchApp.Models
{
    /// <summary>
    /// Represents the model used to convey error information within the BibleSearchApp.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the current request.
        /// This ID can be used to trace and diagnose issues related to the specific request.
        /// Maps to the `RequestId` used in error tracking.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Determines whether to display the Request ID.
        /// Returns <c>true</c> if <see cref="RequestId"/> is not null or empty; otherwise, <c>false</c>.
        /// Useful for conditionally showing the Request ID in error views.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
