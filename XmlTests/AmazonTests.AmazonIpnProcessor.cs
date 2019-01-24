using System.Xml.Linq;

namespace Tests
{
    public partial class AmazonTests
    {
        class AmazonIpnProcessor
        {
            private readonly XNamespace ns = "https://mws.amazonservices.com/ipn/OffAmazonPayments/2013-01-01";

            public AmazonIpnProcessor(string xml)
            {
                var root = XDocument.Parse(xml).Root;
                

                var node = root.Element(ns + "AuthorizationDetails");
                if (node != null)
                {
                    HandleAuthorization(node);
                    return;
                }

                node = root.Element(ns + "OrderReference");
                if (node != null)
                {
                    HandleOrderReference(node);
                    return;
                }

                node = root.Element(ns + "RefundDetails");
                if (node != null)
                {
                    HandleRefund(node);
                    return;
                }
                
            }

            private void HandleRefund(XElement node)
            {
                var statusNode = node.Element(ns + "RefundStatus");
                var status = statusNode.ValueString("State", ns);

                if (status.Equals("completed"))
                {
                    /*
                    refund successfull 
                    */
                }
                else
                {
                    /*
                    Notify for refund 
                    */
                }

            }

            private void HandleOrderReference(XElement node)
            {
                var status = node.ValueString("State", ns);

                if (status.Equals("open"))
                {
                    /*
                    API call:
                    Authorize
                    with
                    Transac!onTimeout = 1440
                    => Asynchronous
                    authoriza!on)*/
                    return;
                }

                if (status.Equals("closed"))
                {
                    /*Payment was completed before*/
                    if (!CheckPaymentStatus(node.Element("AmazonOrderReferenceId").Value))
                    {
                        NotifyPaymentFailed("info");
                    }
                    return;
                }

                /*
                    It is up to you to decide what must be
                    done here. Usually, this should be
                    nothing since you cancelled the order
                    before.
                 */

            }

            private bool CheckPaymentStatus(string value)
            {
                /*
                 was payment successfull ?
                 */
                return true;
            }

            private void HandleAuthorization(XElement node)
            {
                var statusNode = node.Element(ns + "AuthorizationStatus");
                var status = statusNode.ValueString("State", ns);
                if (status.Equals("open"))
                {
                    /*
                     Set Order to
                    Payment Authorized
                    in Shop-Backend or ERP
                    */
                    return;
                }
                var reasonCode = statusNode.ValueString("ReasonCode", ns);

                if (status.Equals("closed"))
                {
                    if (reasonCode.Equals("maxcapturesprocessed"))
                    {
                        /*
                        This indicates a successful capture. When you
                        used CaptureNow:true, this indicates a
                        successful authoriza!on.
                        Set Order to
                        Payment Completed
                        in Shop-Backend or ERP
                        */
                        return;
                    }
                    else
                    {
                        NotifyPaymentFailed("info");
                        return;

                    }
                }
                else
                {
                    if (reasonCode.Equals("invalidpaymentmethod"))
                    {
                        /*
                        Mark order as Payment Suspended.
                        Automa!cally send an Email to the customer
                        to inform him about the failed payment and
                        instruct the customer to visit pay.amazon.com
                        in order to provide another payment method.
                        You will be no!fied via IPN when the
                        customer finished this task.    
                     */
                        return;
                    }
                    else
                    {
                        /*
                         Mark order as Payment Failed
                            Automa!cally send an Email to the customer
                            to inform him about the failed payment. Add
                            a “Please contact our support” to make the
                            customer contact you. Alterna!vely, you
                            might automa!cally cancel this order and
                            inform the customer about it.    
                        */
                        return;
                    }
                }

            }

            private void NotifyPaymentFailed(string v)
            {
                /*
                 Stop any further processing
                 for this order, prevent it from
                 being shipped if s!ll possible
                 */
            }
        }


    }

}