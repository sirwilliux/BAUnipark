Feature: UniparkSteps

Scenario: Book parking
	Given I Open Unipark website
	And Choose the From date to be tomorrow 3 PM
	And Make sure today cannot be selected as To date
	And Set To date to two days from now on and time that is the closest one to the From date
	And Order the parking
	And Select Riga's airport and make sure that the only zone is available
	And Enter car related data
	And Select Vilnius cheapest zone
	And Add last extra service for two adults
	Then Fill all the personal data including all the agreements and options available

	#Optional part:
	#And Refresh the page and make sure that all the data is still present and valid.
	#Then Delete at least one of the mandatory fields and check that at least one error message is displayed.