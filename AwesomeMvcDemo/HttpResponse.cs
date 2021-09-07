using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace AwesomeMvcDemo
{
    /// <summary>
    /// HTTP Response.
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        /// Gets or sets the raw response.
        /// </summary>
        /// <value>
        /// The raw response.
        /// </value>
        public string RawResponse { get; set; }

        /// <summary>
        /// Gets or sets the request URL.
        /// </summary>
        /// <value>
        /// The request URL.
        /// </value>
        public string RequestUrl { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; set; }

    }
}