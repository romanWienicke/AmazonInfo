using NUnit.Framework;
using System;
using System.Collections.Specialized;

namespace Tests
{
    public partial class AmazonTests
    {
        [SetUp]
        public void Setup()
        {
        }




        [Test]
        public void AuthorizationNotificationTest()
        {
            var data = @"<AuthorizationNotification xmlns=""https://mws.amazonservices.com/ipn/OffAmazonPayments/2013-01-01"">
                <AuthorizationDetails>        
	            <AmazonAuthorizationId>S02-0337308-7500736-A034462</AmazonAuthorizationId>        
	            <AuthorizationReferenceId>37956691</AuthorizationReferenceId>        
	            <AuthorizationAmount>          
	            <Amount>43.45</Amount>      
	            <CurrencyCode>EUR</CurrencyCode>    
                </AuthorizationAmount>     
	            <CapturedAmount>          
	            <Amount>0.0</Amount>         
	            <CurrencyCode>EUR</CurrencyCode>     
	            </CapturedAmount>       
	            <AuthorizationFee>       
	            <Amount>0.0</Amount>      
	            <CurrencyCode>EUR</CurrencyCode>      
	            </AuthorizationFee>   
	            <IdList/>    
                <CreationTimestamp>2019-01-24T09:14:57.271Z</CreationTimestamp>  
	            <ExpirationTimestamp>2019-02-23T09:14:57.271Z</ExpirationTimestamp>    
                <AuthorizationStatus>  
	            <State>Declined</State>        
                <LastUpdateTimestamp>2019-01-24T09:14:57.271Z</LastUpdateTimestamp> 
	            <ReasonCode>TransactionTimedOut</ReasonCode>  
	            </AuthorizationStatus>  
	            <SoftDecline>false</SoftDecline>  
	            <OrderItemCategories/>
	            <CaptureNow>true</CaptureNow>     
               <SoftDescriptor>AMZ*Aduis GmbH</SoftDescriptor> 
               </AuthorizationDetails>
            </AuthorizationNotification>";

            var p = new AmazonIpnProcessor(data);
        }

        [Test]
        public void OrderReferenceNotificationTest()
        {
            var data = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                  <OrderReferenceNotification xmlns=""https://mws.amazonservices.com/ipn/OffAmazonPayments/2013-01-01"">
                    <OrderReference>
                    <AmazonOrderReferenceId>
                      S23-1234567-1234567
                    </AmazonOrderReferenceId>
                    <OrderTotal>
                    <Amount>106.00</Amount>
                    <CurrencyCode>GBP</CurrencyCode>
                    </OrderTotal>
                    <SellerNote>4%20Pack%20of%20BBQ%20Sauce</SellerNote>
                    <SellerOrderAttributes>
                      <SellerOrderId>5678-23</SellerOrderId>
                      <StoreName>HammysBurgers.com</StoreName>
                      <CustomInformation>
                         Holiday%20Promotion
                      </CustomInformation>
                    </SellerOrderAttributes>
                    <OrderReferenceStatus>
                    <State>CLOSED</State>
                    <ReasonCode>SellerClosed</ReasonCode>
                    <LastUpdateTimestamp>
                      2013-04-01T10:49:59.532Z
                    </LastUpdateTimestamp>
                    </OrderReferenceStatus>
                    <CreationTimestamp>
                      2013-03-30T09:58:51.234Z
                    </CreationTimestamp>
                    <ExpirationTimestamp>
                      2013-04-06T09:58:51.234Z
                    </ExpirationTimestamp>
                    </OrderReference>
                    </OrderReferenceNotification>";

            var p = new AmazonIpnProcessor(data);
        }



        [Test]
        public void RefundNotificationTest()
        {
            var data = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            <RefundNotification xmlns=""https://mws.amazonservices.com/ipn/OffAmazonPayments/2013-01-01"">
              <RefundDetails>
                <AmazonRefundId>
                  S23-1234567-1234567-0000003
                </AmazonRefundId>
                <RefundReferenceId>
                  07fff0c4e05046958db7e47607e7db17
                </RefundReferenceId>
                <RefundType>SellerInitiated</RefundType>
                <RefundAmount>
                  <Amount>5.0</Amount>
                  <CurrencyCode>GBP</CurrencyCode>
                </RefundAmount>
                <FeeRefunded>
                  <Amount>0.0</Amount>
                  <CurrencyCode>GBP</CurrencyCode>
                </FeeRefunded>
                <CreationTimestamp>
                 2013-04-22T06:07:34.617Z
                </CreationTimestamp>
                <RefundStatus>
                  <State>Completed</State>
                  <LastUpdateTimestamp>
                    2013-04-22T06:09:20.178Z
                  </LastUpdateTimestamp>
                </RefundStatus>
                <SellerRefundNote>
                  Seller%20Refund%20Note
                </SellerRefundNote>
                <SoftDescriptor>
                  AMZ*softDescriptor
                </SoftDescriptor>
              </RefundDetails>
              </RefundNotification>";

            var p = new AmazonIpnProcessor(data);


        }

        [Test]
        public void AmazonIpnHandlerTestOriginalMessage()
        {
            var headers = new NameValueCollection();
            headers.Add("x-amz-sns-message-type", "Notification");
            headers.Add("x-amz-sns-message-id", "5f43584c-1f96-5880-9c98-119f5EXAMPLE");
            headers.Add("x-amz-sns-topic-arn", "arn:aws:sns:EXAMPLE:59860EXAMPLE:TestTopic");
            headers.Add("x-amz-sns-subscription-arn", "arn:aws:sns:EXAMPLE:59860EXAMPLE:TestTopic: EXAMPLE");

            string message = @"{
  ""Type"":""Notification"",
  ""MessageId"":""cf5543af-dd65-5f74-8ccf-0a410EXAMPLE"",
  ""TopicArn"":""arn:aws:sns:EXAMPLE:59860EXAMPLE:TestTopic"",
  ""Message"":
  ""{""NotificationReferenceId"":\
    ""32d195c3-a829-4222-b1e2-14ab2EXAMPLE"",
    ""NotificationType"":""PaymentRefund"",
    ""SellerId"":""YOUR_SELLER_ID_HERE"",
    ""ReleaseEnvironment"":""Sandbox"",
    ""Version"":""2013-01-01"",
    ""NotificationData"":
    ""<?xml version=""1.0"" encoding=""UTF-8""?>
    <RefundNotification xmlns=""https://mws-eu.amazonservices.com/
      ipn/OffAmazonPayments/2013-01-01"">
      <RefundDetails>
        <AmazonRefundId>
          S23-1234567-1234567-0000003
        </AmazonRefundId>
        <RefundReferenceId>
          07fff0c4e05046958db7e47607e7db17
        </RefundReferenceId>
        <RefundType>SellerInitiated</RefundType>
        <RefundAmount>
          <Amount>5.0</Amount>
          <CurrencyCode>GBP</CurrencyCode>
        </RefundAmount>
        <FeeRefunded>
          <Amount>0.0</Amount>
          <CurrencyCode>GBP</CurrencyCode>
        </FeeRefunded>
        <CreationTimestamp>
         2013-04-22T06:07:34.617Z
        </CreationTimestamp>
        <RefundStatus>
          <State>Completed</State>
          <LastUpdateTimestamp>
            2013-04-22T06:09:20.178Z
          </LastUpdateTimestamp>
        </RefundStatus>
        <SellerRefundNote>
          Seller%20Refund%20Note
        </SellerRefundNote>
        <SoftDescriptor>
          AMZ*softDescriptor
        </SoftDescriptor>
      </RefundDetails>
      </RefundNotification>"",
    ""Timestamp"":""2013-04-22T06:00:14Z""}"",
  ""Timestamp"":""2013-04-22T06:00:15.108Z"",
  ""SignatureVersion"":""1"",
  ""Signature"":""kjac14DH...oQT6FbA="",
  ""SigningCertURL"":""https://sns.EXAMPLE.amazonaws.com/
  SimpleNotificationService-f3ecfb7224c7233fe7bb5f59fEXAMPLE.pem"",
  ""UnsubscribeURL"":""https://sns.EXAMPLE.amazonaws.com/
    ?Action=Unsubscribe
    &SubscriptionArn=arn:aws:sns:EXAMPLE:59860EXAMPLE:TestTopic:GUID""
}";

            var handler = new AmazonPay.IpnHandler(headers, message);

        }


        [Test]
        public void AmazonIpnHandlerTestRwMessage()
        {
            var headers = new NameValueCollection();
            headers.Add("x-amz-sns-message-type", "Notification");
            headers.Add("x-amz-sns-message-id", "5f43584c-1f96-5880-9c98-119f5EXAMPLE");
            headers.Add("x-amz-sns-topic-arn", "arn:aws:sns:EXAMPLE:59860EXAMPLE:TestTopic");
            headers.Add("x-amz-sns-subscription-arn", "arn:aws:sns:EXAMPLE:59860EXAMPLE:TestTopic: EXAMPLE");

            string message = @"{
  ""Type"" : ""Notification"",
  ""MessageId"" : ""6d7d23b6-d741-57f7-9ec3-77755310ebf2"",
  ""TopicArn"" : ""arn:aws:sns:eu-west-1:291180941288:A1G8446IYHA4MRA29VRQM4GSQN44"",
  ""Message"" : ""{\""ReleaseEnvironment\"":\""Sandbox\"",\""MarketplaceID\"":\""136311\"",\""Version\"":\""2013-01-01\"",\""NotificationType\"":\""PaymentAuthorize\"",\""SellerId\"":\""A29VRQM4GSQN44\"",\""NotificationReferenceId\"":\""f51edfce-5294-4d3d-aabf-ed2fff0b33ac\"",\""Timestamp\"":\""2019-01-24T09:14:57.735Z\"",\""NotificationData\"":\""<?xml version=\\\""1.0\\\"" encoding=\\\""UTF-8\\\""?><AuthorizationNotification xmlns=\\\""https://mws.amazonservices.com/ipn/OffAmazonPayments/2013-01-01\\\"">\\n    <AuthorizationDetails>\\n        <AmazonAuthorizationId>S02-0337308-7500736-A034462<\\/AmazonAuthorizationId>\\n        <AuthorizationReferenceId>37956691<\\/AuthorizationReferenceId>\\n        <AuthorizationAmount>\\n            <Amount>43.45<\\/Amount>\\n            <CurrencyCode>EUR<\\/CurrencyCode>\\n        <\\/AuthorizationAmount>\\n        <CapturedAmount>\\n            <Amount>0.0<\\/Amount>\\n            <CurrencyCode>EUR<\\/CurrencyCode>\\n        <\\/CapturedAmount>\\n        <AuthorizationFee>\\n            <Amount>0.0<\\/Amount>\\n            <CurrencyCode>EUR<\\/CurrencyCode>\\n        <\\/AuthorizationFee>\\n        <IdList/>\\n        <CreationTimestamp>2019-01-24T09:14:57.271Z<\\/CreationTimestamp>\\n        <ExpirationTimestamp>2019-02-23T09:14:57.271Z<\\/ExpirationTimestamp>\\n        <AuthorizationStatus>\\n            <State>Declined<\\/State>\\n            <LastUpdateTimestamp>2019-01-24T09:14:57.271Z<\\/LastUpdateTimestamp>\\n            <ReasonCode>TransactionTimedOut<\\/ReasonCode>\\n        <\\/AuthorizationStatus>\\n        <SoftDecline>false<\\/SoftDecline>\\n        <OrderItemCategories/>\\n        <CaptureNow>true<\\/CaptureNow>\\n        <SoftDescriptor>AMZ*Aduis GmbH<\\/SoftDescriptor>\\n    <\\/AuthorizationDetails>\\n<\\/AuthorizationNotification>\""}"",
  ""Timestamp"" : ""2019-01-24T09:14:57.746Z"",
  ""SignatureVersion"" : ""1"",
  ""Signature"" : ""Z6+PSiGSDxvyINWLybh/tkYzEZXfIfHlw8AVtPR5STR2gTIlt6rlPTQikr7mbPoGDjr/xg/BlV1ecKrxov1zhNYS0x1jgKRp7s8k4TL6FS0tq/Yx/69GKnhq0lOZrqYyfpwrKep+uVq1g0RIzSXlqLAJKQ9ACUaCP6g1sw9tw5REimop8nLvYLZxR2B8Gx2GRRKis804TUOIeYWCMa/NAoCXVl5lMdnbfSPJAgYMqyIH3nF1Eoilu1dGN49tXxvrtN8e5/lPOwysfdVVfJlg9oFk62Vdr2j4/xgv6AdnW5K4JhqSQG8cN5G9KGo7fn9WZYsAbPm15qplw+1ZrTmu5g=="",
  ""SigningCertURL"" : ""https://sns.eu-west-1.amazonaws.com/SimpleNotificationService-ac565b8b1a6c5d002d285f9598aa1d9b.pem"",
  ""UnsubscribeURL"" : ""https://sns.eu-west-1.amazonaws.com/?Action=Unsubscribe&SubscriptionArn=arn:aws:sns:eu-west-1:291180941288:A1G8446IYHA4MRA29VRQM4GSQN44:e5327081-fa1e-4e1b-86ee-8d97d274aadb""
}";

            var handler = new AmazonPay.IpnHandler(headers, message);

        }
    }

}