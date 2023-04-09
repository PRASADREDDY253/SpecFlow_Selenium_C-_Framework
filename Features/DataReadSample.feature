Feature: TestData Reading Json and Excel

A short summary of the feature

@TD
Scenario Outline: Read And Display JSON
	Then I enter credentials for "<User>" from the file "Login.json" and click login
	Examples: 
	| User     |
	| CoreUser |

Scenario Outline: Excel Read And Display
	Then I read data from excel sheet "<Sheetname>" for scenario "<ScenarioName>"

	Examples: 
	| Sheetname   | ScenarioName |
	| DataSample  | Login        |
