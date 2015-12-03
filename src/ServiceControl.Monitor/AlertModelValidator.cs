//namespace EaiGuy.ServiceControl.Monitor
//{
//    using Messages.Events;
//    using System;
//    using System.Linq;

//    public static class AlertModelValidator
//    {
//        /// <summary>
//        /// In order to make errors more transparent, this method steps through each data point and formatting rule that the cshtml file does
//        /// </summary>
//        public static void Validate(this IHealthStatusChanged scUpdated)
//        {
//            if (scUpdated.Endpoints.Any())
//            {
//                string endpointString = String.Empty;
//                foreach (var endpointTuple in scUpdated.Endpoints)
//                {
//                    endpointString += endpointTuple.Item2 + " ";
//                    endpointString += endpointTuple.Item1.HostDisplayName + " ";
//                    endpointString += endpointTuple.Item1.Name + " ";
//                    endpointString += endpointTuple.Item1.HeartbeatInformation.ReportedStatus.ToString() + " ";
//                    endpointString += endpointTuple.Item1.HeartbeatInformation.LastReportAt.ToString("g") + "\r\n";
//                    endpointString = String.Empty;
//                }
//            }

//            if (scUpdated.CustomChecks.Any())
//            {
//                string checkString = String.Empty;
//                foreach (var checkTuple in scUpdated.CustomChecks)
//                {
//                    checkString += checkTuple.Item2 + " ";
//                    checkString += checkTuple.Item1.OriginatingEndpoint.Host + " ";
//                    checkString += checkTuple.Item1.OriginatingEndpoint.Name + " ";
//                    checkString += checkTuple.Item1.CustomCheckId + " ";
//                    checkString += checkTuple.Item1.Status.ToString() + " ";
//                    checkString += checkTuple.Item1.ReportedAt.ToString("g") + " ";
//                    checkString += checkTuple.Item1.FailureReason + "\r\n";
//                    checkString = String.Empty;
//                }
//            }
//        }
//    }
//}
