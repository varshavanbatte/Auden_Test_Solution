using Google.Protobuf.WellKnownTypes;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;

namespace ShortTermLoan.StepBindings
{
    [Binding]
    public class LoanEligibilitySteps : IDisposable
    {
        private ChromeDriver chromeDriver;

        public object JavaScriptExecutor { get; private set; }

        public LoanEligibilitySteps() => chromeDriver = new ChromeDriver();

        [Given(@"user navigates to Auden Loan website")]
        public void GivenUserNavigatesToAudenLoanWebsite()
        {
            chromeDriver.Navigate().GoToUrl("https://www.auden.co.uk/credit/shorttermloan");
            chromeDriver.Manage().Window.Maximize();
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            Assert.IsTrue(chromeDriver.Title.ToLower().Contains("auden"));
        }
        

        [Given(@"user verifies minimum amount of loan slider is ""(.*)""")]
        public void GivenUserVerifiesMinimumAmountOfLoanSliderIs(int minLoanAmount)
        {
            IWebElement slider = chromeDriver.FindElementByXPath("/html/body/div/div/div/div[1]/div/div[2]/div[1]/section[1]/div/label/input");
            int minAmount = Int32.Parse(slider.GetAttribute("min"));
            Assert.AreEqual(minLoanAmount, minAmount);
        }

        [Given(@"user verifies maximum amount of loan slider is ""(.*)""")]
        public void GivenUserVerifiesMaximumAmountOfLoanSliderIs(int maxLoanAmount)
        {
            IWebElement slider = chromeDriver.FindElementByXPath("/html/body/div/div/div/div[1]/div/div[2]/div[1]/section[1]/div/label/input");
            int maxAmount = Int32.Parse(slider.GetAttribute("max"));
            Assert.AreEqual(maxLoanAmount, maxAmount);
        }
                     
        [Given(@"user selects ""(.*)"" on loan calculator slider")]
        public void GivenUserSelectsOnLoanCalculatorSlider(int loanAmount)
        {
            IWebElement slider = chromeDriver.FindElementByXPath("/html/body/div/div/div/div[1]/div/div[2]/div[1]/section[1]/div/label/input");
            int selectedloanAmount = Int32.Parse(slider.GetAttribute("value"));
 
            while (selectedloanAmount != loanAmount)
            {
                slider.SendKeys(Keys.ArrowRight);
                selectedloanAmount = Int32.Parse(slider.GetAttribute("value"));
            }

            Assert.AreEqual(loanAmount, selectedloanAmount);           
        }

        [Given(@"user verifies ""(.*)"" is displayed on loan amount")]
        public void GivenUserVerifiesIsDisplayedOnLoanAmount(string loanAmount)
        {
            IWebElement slider = chromeDriver.FindElementByXPath("/html/body/div/div/div/div[1]/div/div[2]/div[1]/section[1]/header/div[2]/span");
            Assert.AreEqual(loanAmount, slider.Text);
        }

        [When(@"user taps ""(.*)"" button on loan calculator schedule")]
        public void WhenUserTapsButtonOnLoanCalculatorSchedule(int date)
        {
            IWebElement dateSelector = chromeDriver.FindElementsByClassName("date-selector__date")[date-1];
            dateSelector.Click();
        }

        [When(@"user swipes down")]
        public void WhenUserSwipesDown()
        {
            Thread.Sleep(3000);
            ((IJavaScriptExecutor)chromeDriver).ExecuteScript("scroll(0,300)");
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [Then(@"user sees ""(.*)"" under First repayment")]
        public void ThenUserSeesUnderFirstRepayment(string firstRepaymentDate)
        {
            IWebElement repayment = chromeDriver.FindElementByClassName("loan-schedule__tab__panel__detail__tag__label");
            Assert.AreEqual(firstRepaymentDate, repayment.Text);
        }

        [Then(@"user sees ""(.*)"" on loan calculator summary amount")]
        public void ThenUserSeesOnLoanCalculatorSummaryAmount(string loanAmount)
        {
            Thread.Sleep(3000);
            loanAmount = loanAmount + ".";
            IWebElement amount = chromeDriver.FindElementByXPath("/html/body/div/div/div/div[1]/div/div[2]/div[1]/section[3]/div[1]/div/span[1]");
            Assert.AreEqual(loanAmount, amount.Text);
        }


        public void Dispose()
        {
            if (chromeDriver != null)
            {
                chromeDriver.Dispose();
                chromeDriver = null;
            }
        }
    }
}
