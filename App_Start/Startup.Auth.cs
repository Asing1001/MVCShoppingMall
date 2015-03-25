using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using IdentitySample.Models;
using Owin;
using System;
using Microsoft.Owin.Security.Google;

namespace IdentitySample
{
    public partial class Startup
    {
        // 如需設定驗證的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // 設定資料庫內容、使用者管理員和登入管理員，以針對每個要求使用單一執行個體
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
          
            // 讓應用程式使用 Cookie 儲存已登入使用者的資訊
            // 並使用 Cookie 暫時儲存使用者利用協力廠商登入提供者登入的相關資訊；
            // 在 Cookie 中設定簽章
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // 讓應用程式在使用者登入時驗證安全性戳記。
                    // 這是您變更密碼或將外部登入新增至帳戶時所使用的安全性功能。  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 讓應用程式在雙因素驗證程序中驗證第二個因素時暫時儲存使用者資訊。
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // 讓應用程式記住第二個登入驗證因素 (例如電話或電子郵件)。
            // 核取此選項之後，將會在用來登入的裝置上記住登入程序期間的第二個驗證步驟。
            // 這類似於登入時的 RememberMe 選項。
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // 註銷下列各行以啟用利用協力廠商登入提供者登入
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
            appId: "292696360926184",
            appSecret: "42689850073ee1ef6af3ad0029cbec13");


            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                //ClientId = "183168110639-oo3lfjitqjmeaubbsjdq451jakvaun3q.apps.googleusercontent.com",
                //ClientSecret = "PvKxWkAu6rklls5wHmxEXHRD"
                //for https://wecarestore.azurewebsites.net/
                ClientId = "183168110639-fk7jh3ra2hm2sivjkafavn7t96iudlla.apps.googleusercontent.com",
                ClientSecret = "u_t8ctNMuctbCn4_3rAGD8aD"
                //            );
                //            //for http://mvctest0917.azurewebsites.net/
             //ClientId= "183168110639-crarj6j8cnh367lrhu36cio1clg0hur7.apps.googleusercontent.com",
             //ClientSecret=    "XPoA21NdMMyLMdtUk4ZIo1df"

                //            // for https://mvc0918.azurewebsites.net/
                //            //clientId:"183168110639-v122ktp9enpat3009rvb2oqvj3t8abs7.apps.googleusercontent.com",
                //            //clientSecret:"J3coYv4WBXSAttA6r1cGsLrM";)
                ////for https://mvc0919.azurewebsites.net/
                //            // clientId: "183168110639-ut86t4lqhpk3tsakuj90tpfnifp1acnr.apps.googleusercontent.com",
                //            //clientSecret:    "yCJ2yI4wM3tCAC2o-sllUmnS);
            });
        }
    }
        //    }
        //    public partial class Startup
        //    {
        //        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        //        public void ConfigureAuth(IAppBuilder app)
        //        {
        //            // Configure the db context, user manager and role manager to use a single instance per request
        //            app.CreatePerOwinContext(ApplicationDbContext.Create);
        //            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        //            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
        //            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

        //            // Enable the application to use a cookie to store information for the signed in user
        //            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
        //            // Configure the sign in cookie
        //            app.UseCookieAuthentication(new CookieAuthenticationOptions
        //            {
        //                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        //                LoginPath = new PathString("/Account/Login"),
        //                Provider = new CookieAuthenticationProvider
        //                {
        //                    // Enables the application to validate the security stamp when the user logs in.
        //                    // This is a security feature which is used when you change a password or add an external login to your account.  
        //                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
        //                        validateInterval: TimeSpan.FromMinutes(30),
        //                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        //                }
        //            });
        //            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        //            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
        //            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

        //            // Enables the application to remember the second login verification factor such as phone or email.
        //            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
        //            // This is similar to the RememberMe option when you log in.
        //            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

        //            // Uncomment the following lines to enable logging in with third party login providers
        //            //app.UseMicrosoftAccountAuthentication(
        //            //    clientId: "",
        //            //    clientSecret: "");

        //            //app.UseTwitterAuthentication(
        //            //   consumerKey: "",
        //            //   consumerSecret: "");

        //            app.UseFacebookAuthentication(
        //            appId: "351051355069147",
        //            appSecret: "9def0cceec52a42a4c5f241b9be6b55e");

        //          app.UseGoogleAuthentication(
        //              //for https://wecarestore.azurewebsites.net/
        //                clientId:"183168110639-fk7jh3ra2hm2sivjkafavn7t96iudlla.apps.googleusercontent.com",
        //                clientSecret: "u_t8ctNMuctbCn4_3rAGD8aD"
        //            );
        //            //for http://mvctest0917.azurewebsites.net/
        //            //clientId: "183168110639-crarj6j8cnh367lrhu36cio1clg0hur7.apps.googleusercontent.com",
        //            //clientSecret:    "XPoA21NdMMyLMdtUk4ZIo1df);

        //            // for https://mvc0918.azurewebsites.net/
        //            //clientId:"183168110639-v122ktp9enpat3009rvb2oqvj3t8abs7.apps.googleusercontent.com",
        //            //clientSecret:"J3coYv4WBXSAttA6r1cGsLrM";)
        ////for https://mvc0919.azurewebsites.net/
        //            // clientId: "183168110639-ut86t4lqhpk3tsakuj90tpfnifp1acnr.apps.googleusercontent.com",
        //            //clientSecret:    "yCJ2yI4wM3tCAC2o-sllUmnS);
        //        }
        //    }
    
}