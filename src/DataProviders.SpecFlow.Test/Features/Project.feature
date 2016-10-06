Feature: Project

Scenario: Seeding countries and projects
    Given the following entries of Country exist
	| Id | Name   |
	| N  | North  |
	| S  | South  |
	| L  | London |
    And the following entries of Project exist
	| Id | Name            | CountryId |
	| P1 | Increase budget | N         |
	| P2 | Move to USA     | S         |
    | P3 | Decrease tools  | S         |