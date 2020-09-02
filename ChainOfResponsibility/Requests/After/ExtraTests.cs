using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesignPatterns.ChainOfResponsibility.Requests.After
{
    [TestClass]
    public class ExtraTests
    {
        [TestMethod]
        public void user_that_belong_to_right_role_should_get_access()
        {
            var response = new Response
            {
                Page = new Page
                {
                    PageType = PageType.Politics
                }
            };

            var c = new CorrectRoleGivesAccess();
            c.Handle(
                new Request
                {
                    User = new User
                    {
                        
                        Roles = new[] { "PoliticsEditor" }
                    },
                }
                , 
                response
            );

            Assert.IsTrue(response.Authorized);
            
        }

        [TestMethod]
        public void user_that_dont_belong_to_role_should__not_get_access()
        {
            var response = new Response
            {
                Page = new Page
                {
                    PageType = PageType.Politics
                }
            };

            var c = new CorrectRoleGivesAccess();
            c.Handle(
                new Request
                {
                    User = new User
                    {

                        Roles = new[] { "SportsEditor" }
                    },
                }
                ,
                response
            );

            Assert.IsFalse(response.Authorized);

        }
    }
}
