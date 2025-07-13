Feature: Patient Identification with NHS Number, Surname, and Date of Birth
		In order to ensure accurate patient identification
		As a healthcare provider
		I want to verify patient details using their NHS number, surname, and date of birth.

  Scenario: Patient identification using NHS number, surname, and date of birth
		Given the patient has an NHS number "222333444"
		And the patient has a surname "Smith"
		And the patient has a date of birth "02-03-2000"
		When the patient is identified
		Then the identification should be successful
		And the system should return "222333444", "Alice", "Smith", "02-03-2000"

  Scenario: Patient identification with wrong NHS number
		Given the patient has an NHS number "111222333"
		And the patient has a surname "Smith"
		And the patient has a date of birth "02-03-2000"
		When the patient is identified
		Then the identification should fail
		And the system should return an error message "Your details could not be found"

  Scenario: Patient identification with wrong surname
	    Given the patient has an NHS number "222333444"
	    And the patient has a surname "Johnson"
	    And the patient has a date of birth "02-03-2000"
	    When the patient is identified
	    Then the identification should fail
	    And the system should return an error message "Your details could not be found"

  Scenario: Patient identification with wrong date of birth
        Given the patient has an NHS number "222333444"
        And the patient has a surname "Smith"
        And the patient has a date of birth "02-03-1999"
        When the patient is identified
        Then the identification should fail
        And the system should return an error message "Your details could not be found"
  
  Scenario: Patient identification with patient under age of 16
		Given the patient has an NHS number "555666777"
		And the patient has a surname "May"
		And the patient has a date of birth "14-11-2010"
		When the patient is identified
		Then the identification should fail
		And the system should return an error message "You are not eligble for this service"

  Scenario: Patient identification with wrong NHS number, surname, and date of birth
        Given the patient has an NHS number "111222333"
		And the patient has a surname "Johnson"
		And the patient has a date of birth "02-03-1999"
		When the patient is identified
		Then the identification should fail
		And the system should return an error message "Your details could not be found"