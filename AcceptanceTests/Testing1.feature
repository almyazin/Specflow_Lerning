Feature: Testing1 API
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

Scenario: User receives events count for a project
	Given A process <ProcessId>
	And A project <ProjectId> in the process
	And A user with <Project member> process role and <Project member> project role in the project
	And The user has been authenticated
	When The user receives events count for the project
	Then The count is equal to count of undone events in the project

Scenario: Project member creates project event with only required information
	Given A process <ProcessId>
	And A project <ProjectId> in the process
	And A user with <Project member> process role and <Project member> project role in the project
	And The user has been authenticated
	When The user creates an event for the project <ProjectId> with title <Event_1>
	Then The event is created successfully with this information:
		| attributeName | attributeValue |
		| Title         | Event_1        |
		| Type          | Meeting        |
		| StartDate     | creation_date  |
		| EndDate       | creation_date  |
		| Note          |                |
		| Important     | false          |
		| Done          | false          |
		| Responsible   |                |

Scenario: Just test a runner
	When I do something
	Then I huhuhu

Scenario Outline: I use examples table
	Given I am
	When I use <key>
	Then I get <value>	
	
	Examples:
		| key  | value       |
		| key1 | value1      |
		| key2 | value2      |
		| key3 | wrong_value |