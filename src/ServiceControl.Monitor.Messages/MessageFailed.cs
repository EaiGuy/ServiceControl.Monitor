//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EaiGuy.ServiceControl.Monitor.Messages
//{
//    public class MessageFailed
//    {
//            //    <th style="width:15%;">Host</th>
//            //    <th style="width:20%;">Endpoint</th>
//            //    <th>Message Type</th>
//            //    <th style="width:15%;">Reported At</th>
//            //    <th style="width:20%;">Failure Reason</th>
//            //</tr>
//            //@foreach (var failedMsg in Model.FailedMessages)
//            //{
//            //    <tr class="@(RowStatus.JustBroke)">
//            //        <td>@(failedMsg.ProcessingEndpoint.Host)</td>
//            //        <td>@(failedMsg.ProcessingEndpoint.Name.Replace(".", " "))</td>
//            //        <td>@(failedMsg.MessageType.Replace(".", " "))</td>
//            //        <td>@(failedMsg.FailureDetails.TimeOfFailure.ToString("g"))</td>
//            //        <td>@(failedMsg.FailureDetails.Exception.Message)</td>

//        public string ProcessingEndpointHost { get; set; }
//        public string ProcessingEndpointName { get; set; }
//        public string MessageType { get; set; }
//        public DateTime TimeOfFailure { get; set; }
//        public string ExceptionMessage { get; set; }
//    }
//}
