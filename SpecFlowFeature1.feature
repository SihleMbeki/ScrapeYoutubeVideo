Feature: Extract video comments
	In order to extract video comments
	As a researcher
	I want to extract user comments

Scenario Outline:  extract video comments
	Given the video url is valid
	When I open the video "<url>"
	And extract video comments
	Then the results file should be created

Examples: 
|url                                                                          |
|https://www.youtube.com/watch?v=mrQ5lQxNImk                                  |
|https://www.youtube.com/watch?v=IeTgNGk0lXo&list=RD7pBwsdUKE98&index=2       |
|https://www.youtube.com/watch?v=7pBwsdUKE98&list=RD7pBwsdUKE98&start_radio=1|