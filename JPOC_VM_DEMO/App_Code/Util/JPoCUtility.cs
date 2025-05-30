using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Jpoc.Common;
using Jpoc.Dac;
using GlobalVariables = JPOC_VM_DEMO.Common.GlobalVariables;

namespace JPOC_VM_DEMO.App_Code.Util
{
    public class JPoCUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<JPoCUtility> _logger;

        // Singleton instance (if needed)
        //private static JPoCUtility _instance;
        //public static JPoCUtility Instance => _instance ??= new JPoCUtility(
        //    // You must provide these via DI in Startup/Program.cs
        //    // Example: services.AddSingleton<JPoCUtility>();
        //    // Or use DI everywhere and avoid static instance
        //    null, null
        //);

        // Example fields
        private string _pSText;
        private string _pSTerm;

        // Constructor for DI
        public JPoCUtility(IHttpContextAccessor httpContextAccessor, ILogger<JPoCUtility> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        // Example: Logout logic
        public void Logout(bool endResponse = true)
        {
            // Implement logout logic using _httpContextAccessor.HttpContext
        }

        // Example: Convert link tags
        public string ConvLinkTag(string inputHtml, int diseaseId, string searchTerm, string searchText, HttpRequest request, bool noTagConvert, bool isPrintPage, bool isHideTag = false, bool skipRecursiveRetrieval = false, bool fromEmbedImage = false)
        {
            // Implement logic
            return string.Empty;
        }

        /// <summary>
        /// Gets a message from the configuration using the specified message code and optional format parameters
        /// </summary>
        /// <param name="messageCode">The message code to lookup</param>
        /// <param name="values">Optional format parameters to substitute into the message</param>
        /// <returns>The formatted message</returns>
        public string GetMessage(string messageCode, params string[] values)
        {
            var messageSet = GlobalVariables.Messages.GetMessageSet(messageCode);
            // Replace values in message if provided
            if (values != null && values.Length > 0)
            {
                messageSet.Message = string.Format(messageSet.Message, values);
            }
            return messageSet.Message;
        }

        /// <summary>
        /// Gets a message set from the configuration using the specified message code and optional format parameters
        /// </summary>
        /// <param name="messageCode">The message code to lookup</param>
        /// <param name="values">Optional format parameters to substitute into the message</param>
        /// <returns>The MessageSet containing the formatted message</returns>
        public MessageSet GetMessageSet(string messageCode, params string[] values)
        {
            var messageSet = GlobalVariables.Messages.GetMessageSet(messageCode);
            // Replace values in message if provided
            if (values != null && values.Length > 0)
            {
                messageSet.Message = string.Format(messageSet.Message, values);
            }
            return messageSet;
        }


        /// <summary>
        /// Converts a list of messages into a single string with proper line breaks
        /// </summary>
        /// <param name="messageList">List of messages to convert</param>
        /// <param name="isJavaScriptFormat">Whether to format for JavaScript output</param>
        /// <returns>Combined message string with appropriate line breaks</returns>
        public string ConvMessage(List<string> messageList, bool isJavaScriptFormat = true)
        {
            if (messageList == null || !messageList.Any())
                return string.Empty;

            var message = string.Join(Environment.NewLine, messageList);

            if (isJavaScriptFormat)
            {
                // Replace all types of line breaks with JavaScript line break
                return message
                    .Replace("\r\n", @"\n")
                    .Replace("\r", @"\n")
                    .Replace("\n", @"\n")
                    .Replace("<br />", @"\n");
            }
            else
            {
                // Convert JavaScript line breaks to platform-specific line breaks
                return message
                    .Replace(@"\n", Environment.NewLine)
                    .Replace(@"\r", string.Empty);
            }
        }


        // Example: Send email
        public bool SendEmail(
            ref string errMsg,
            string fromAdd,
            string mailSubject,
            List<string> toAdd,
            List<string> ccAdd,
            bool isHTMLMail,
            string mailContent,
            string attatchMentFilePath = null,
            string attatchMentFileName = null,
            bool isIPRecognised = false)
        {
            // Implement email logic using SmtpClient
            return true;
        }

        // Example: Exception manager
        public void ExceptionManager(Exception ex, bool isAsyncBatch = false)
        {
            // Implement exception handling logic
        }

        // Example: Session helpers
        public string GetSession(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }
        public void SetSession(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }
        public void RemoveSession(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }

        // Example: File download (should be in a controller)
        // public FileResult DownloadFile(string filePath, string fileName)
        // {
        //     var bytes = System.IO.File.ReadAllBytes(filePath);
        //     return File(bytes, "application/octet-stream", fileName);
        // }

        // ... Add other methods, adapting signatures as needed for .NET Core ...

        // Example: Static helper for DataTable conversion
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            // Implement conversion logic
            return new DataTable();
        }

        // Example: Use HtmlAgilityPack for HTML parsing
        public int GetHtmlContentsLength(string htmlText)
        {
            // Implement logic using HtmlAgilityPack
            return 0;
        }

        // ... Continue with other methods, adapting for .NET Core ...
    }
}
