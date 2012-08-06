using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace IntegrationTests
{
    [Binding]
    public class VisitTrackingSteps
    {
        private HttpResponseMessage _result;
        private string _uri;

        [Given(@"the api uri is (.*)")]
        public void GivenTheApiUriIs(string uri)
        {
            _uri = uri;
        }

        [When(@"I hit the visit tracking uri")]
        public void WhenIHitTheVisitTrackingUri()
        {
            var url = "http://" + _uri;
            var client = new HttpClient();
            var msg = new HttpRequestMessage(HttpMethod.Get, url);

            _result = client.SendAsync(msg).Result;
        }

        [Then(@"the response HttpCode is (.*)")]
        public void ThenTheResponseHttpCodeIs(HttpStatusCode statusCode)
        {
            Assert.That(_result.StatusCode, Is.EqualTo(statusCode));
        }

        [Then(@"the response sets a cookie")]
        public void ThenTheResponseSetsACookie()
        {
            var isSetCookieHeaderPresent = !String.IsNullOrEmpty(GetValueOfSetCookieHeader());
            Assert.IsTrue(isSetCookieHeaderPresent);
        }

        [Then(@"the cookie name is (.*)")]
        public void ThenTheCookieNameIs(string expectedCookieName)
        {
            var cookieName = ParseSetCookieValue().AllKeys.FirstOrDefault();
            Assert.That(cookieName, Is.EqualTo(expectedCookieName));
        }

        [Then(@"the value for (.*) is a valid Guid")]
        public void ThenTheValueForIsAValidGuid(string expectedCookieName)
        {
            Guid guid;
            var guidValue = ParseSetCookieValue()[expectedCookieName];
            var isValidGuid = Guid.TryParse(guidValue, out guid);
            
            Assert.IsTrue(isValidGuid);
        }

        [Then(@"the cookie expiry is (.*) days from now")]
        public void ThenTheCookieExpiryIsDaysFromNow(int days)
        {
            var expiresValue = ParseSetCookieValue()["expires"];
            var expires = Convert.ToDateTime(expiresValue);
            var rangeStart = DateTime.Now.AddDays(7).AddMinutes(-1);
            var rangeEnd = DateTime.Now.AddDays(7).AddMinutes(1);

            Assert.That(expires, Is.InRange(rangeStart, rangeEnd));
        }

        private string GetValueOfSetCookieHeader()
        {
            IEnumerable<string> values;
            _result.Headers.TryGetValues("Set-Cookie", out values);

            return values.FirstOrDefault();
        }

        private NameValueCollection ParseSetCookieValue()
        {
            var collection = new NameValueCollection();
            var cookieValArray = GetValueOfSetCookieHeader().Split(';');
            
            foreach (var arr in cookieValArray.Select(s => s.Split('=')))
            {
                collection.Add(arr[0].Trim(), arr[1].Trim());
            }

            return collection;
        }
    }
}