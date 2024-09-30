@OrangeHRM
Feature: OrangeHRM


Scenario: Verify filter screen in Admin Page
	Given Open the OrangeHRM website
	When Enter username and password
	And Click on SignIn button
	And Select the "Admin" Option from menu
	And Enter the filter inputs "FirstName" , "Lastname"
	Then Observer the record
