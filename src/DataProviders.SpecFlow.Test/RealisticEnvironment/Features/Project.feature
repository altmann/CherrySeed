Feature: Project

Scenario: Seeding countries and projects
    Given the following countries exist
	| Id | Name   |
	| N  | North  |
	| S  | South  |
	| L  | London |
    And the following projects exist
	| Id | Name            | CountryId |
	| P1 | Increase budget | N         |
	| P2 | Move to USA     | S         |
    | P3 | Decrease tools  | S         |