Feature: LoanEligibility
	In order to Loan Eligibility on Auden website
	As a tester
	I want to ensure functionality is working end to end

@mytag
Scenario: User should be able to select loan amount other than default and Repayment day as weekend
	Given user navigates to Auden Loan website
	And user verifies minimum amount of loan slider is "200"
	And user verifies maximum amount of loan slider is "500"
	And user selects "250" on loan calculator slider
	When user taps "8" button on loan calculator schedule
	And user swipes down
	Then user sees "Friday 7 Aug 2020" under First repayment
	And user sees "£250" on loan calculator summary amount
	
