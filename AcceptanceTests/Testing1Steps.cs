using NUnit.Framework;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    [Binding]
    public class Testing1Steps
    {
        Dictionary<string, string> Iam = new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" }, { "key3", "value3" } };

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            ScenarioContext.Current.Pending();
        }


        [Given(@"A process (.*)")]
        public void GivenAProcess(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"A project (.*) in the process")]
        public void GivenAProjectInTheProcess(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"A user with (.*) process role and (.*) project role in the project")]
        public void GivenAUserWithProcessRoleAndProjectRoleInTheProject(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"The user has been authenticated")]
        public void GivenTheUserHasBeenAuthenticated()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"The user receives events count for the project")]
        public void WhenTheUserReceivesEventsCountForTheProject()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"The count is equal to count of undone events in the project")]
        public void ThenTheCountIsEqualToCountOfUndoneEventsInTheProject()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"The user creates an event for the project (.*) with title (.*)")]
        public void WhenTheUserCreatesAnEventForTheProjectWithTitle(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"The event is created successfully with this information:")]
        public void ThenTheEventIsCreatedSuccessfullyWithThisInformation(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I am")]
        public void GivenIAm()
        {
            ScenarioContext.Current.Add("Iam", Iam);
        }

        [When(@"I use (.*)")]
        public void WhenIUse(string key)
        {
            ScenarioContext.Current["key"] = key;
        }

        [Then(@"I get (.*)")]
        public void ThenIGet(string value)
        {
            var key = ScenarioContext.Current["key"] as string;
            var Iam = ScenarioContext.Current["Iam"] as Dictionary<string, string>;
            Assert.AreEqual(value, Iam[key]);
        }

        [When(@"I do something")]
        public void WhenIDoSomething() {}

        [Then(@"I huhuhu")]
        public void ThenIHuhuhu()
        {
            Assert.True(true);
        }


    }
}
