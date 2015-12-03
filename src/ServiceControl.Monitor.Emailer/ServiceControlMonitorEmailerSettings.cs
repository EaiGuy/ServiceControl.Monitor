namespace EaiGuy.ServiceControl.Monitor.Emailer
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class ServiceControlMonitorEmailerSettings
    {
        internal string ErrorEmailAddress { get { return ConfigurationManager.AppSettings["ErrorEmailAddress"]; } }

        // TODO: Flesh out implementation of endpoint-specific error routing
        ///// <summary>
        ///// Returns a list of tuples where Item1 = the name of the endpoint and Item2 = a comma-delimited list of email addresses
        ///// </summary>
        //public List<Tuple<string, string>> EndpointSpecificErrorEmailAddresses
        //{
        //    get
        //    {
        //        char[] separator = new char[] { ':' };
        //        string settingsNames = ConfigurationManager.AppSettings["EndpointSpecificErrorEmailAddresses"] ?? String.Empty;
        //        List<string> settingsNameList = settingsNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        //        return settingsNameList.Select(x => Tuple.Create(
        //                x.Split(separator).First(),
        //                x.Split(separator).Skip(1).First())
        //            ).ToList();
        //    }
        //}
    }
}
