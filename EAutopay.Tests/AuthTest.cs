﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAutopay.Security;
using EAutopay.Tests.Fakes;

namespace EAutopay.Tests
{
    [TestClass]
    public class AuthTest
    {
        FakeConfig _config;
        FakeCrawler _crawler;
        Auth _auth;

        [TestMethod]
        public void Login_Should_Return_Failed_If_No_Uri()
        {
            InitAuth();

            var resp = _auth.Login();
            Assert.AreEqual(AuthStatus.Login_Failed, resp.Status);
        }

        [TestMethod]
        public void Login_Should_Return_OK_For_MainUri()
        {
            InitAuth();
            _crawler.SetResponseUri(Common.MAIN_URI);

            var resp = _auth.Login();
            Assert.AreEqual(AuthStatus.Ok, resp.Status);
        }

        [TestMethod]
        public void Login_Should_Post_Secret_If_SecretUri()
        {
            InitAuth();

            // simulate redirection to the secret page after loggin in
            _crawler.SetResponseUri(Common.SECRET_URI);

            _auth.Login();

            bool wasSecretPageLoaded = _crawler.Paramz["secret_answer"] =="test_secret";
            Assert.IsTrue(wasSecretPageLoaded);
        }

        [TestMethod]
        public void Login_Should_Not_Post_Secret_For_MainUri()
        {
            InitAuth();
            _crawler.SetResponseUri(Common.MAIN_URI);

            _auth.Login();

            bool wasSecretPageLoaded = _crawler.Paramz["secret_answer"] == "test_secret";
            Assert.IsFalse(wasSecretPageLoaded);
        }

        [TestMethod]
        public void Logout_Should_Send_To_Correct_Uri()
        {
            InitAuth();
            _config.SetLogin("login");

            _auth.Logout();

            Assert.AreEqual(Common.LOGOUT_URI, _crawler.Uri);
        }

        [TestMethod]
        public void IsLogged_Returns_True_For_MainUri()
        {
            InitAuth();
            _crawler.SetResponseUri(Common.MAIN_URI);

            Assert.IsTrue(_auth.IsLogged);
        }

        [TestMethod]
        public void IsLogged_Returns_False_For_Other_Uri()
        {
            InitAuth();
            _crawler.SetResponseUri(Common.FORMS_URI);

            Assert.IsFalse(_auth.IsLogged);
        }

        public void InitAuth()
        {
            _config = new FakeConfig();
            _crawler = new FakeCrawler();
            _auth = new Auth("test_login", "test_password", "test_secret", _config, _crawler);
        }
    }
}
