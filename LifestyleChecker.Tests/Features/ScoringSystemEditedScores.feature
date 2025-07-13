Feature: Calculate Risk Score
		In order to assess health risks
		As a healthcare provider
		I want to calculate a risk score based on binary inputs to questionnaire lifestyle factors.

  Scenario: Calculate risk score with all medically negative inputs and age bracket 16-21
		Given I patient aged 18 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with true
		And I answer the question 'Do you exercise more than 1 hour per week' with false
		When I submit the questionnaire
		Then I should see the risk score 5
		And display the message "We think there are some simple things you could do to improve you quality of life, please phone to book an appointment"

  Scenario: Calculate risk score with all true inputs and age bracket 16-21
		Given I patient aged 18 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with true
		And I answer the question 'Do you exercise more than 1 hour per week' with true
		When I submit the questionnaire
		Then I should see the risk score 3
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with all false inputs and age bracket 16-21
		Given I patient aged 18 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with false
		And I answer the question 'Do you smoke' with false
		And I answer the question 'Do you exercise more than 1 hour per week' with false
		When I submit the questionnaire
		Then I should see the risk score 2
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with mixed inputs and age bracket 16-21
		Given I patient aged 18 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with false
		And I answer the question 'Do you exercise more than 1 hour per week' with true
		When I submit the questionnaire
		Then I should see the risk score 2
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with all medically negative inputs and age bracket 22-40
		Given I patient aged 25 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with true
		And I answer the question 'Do you exercise more than 1 hour per week' with false
		When I submit the questionnaire
		Then I should see the risk score 4
		And display the message "We think there are some simple things you could do to improve you quality of life, please phone to book an appointment"

  Scenario: Calculate risk score with all true inputs and age bracket 22-40
		Given I patient aged 25 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with true
		And I answer the question 'Do you exercise more than 1 hour per week' with true
		When I submit the questionnaire
		Then I should see the risk score 3
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with all false inputs and age bracket 22-40
		Given I patient aged 25 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with false
		And I answer the question 'Do you smoke' with false
		And I answer the question 'Do you exercise more than 1 hour per week' with false
		When I submit the questionnaire
		Then I should see the risk score 1
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with mixed inputs and age bracket 22-40
		Given I patient aged 25 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with false
		And I answer the question 'Do you exercise more than 1 hour per week' with true
		When I submit the questionnaire
		Then I should see the risk score 2
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with all medically negative inputs and age bracket 41-65
		Given I patient aged 50 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with true
		And I answer the question 'Do you exercise more than 1 hour per week' with false
		When I submit the questionnaire
		Then I should see the risk score 6
		And display the message "We think there are some simple things you could do to improve you quality of life, please phone to book an appointment"

  Scenario: Calculate risk score with all true inputs and age bracket 41-65
		Given I patient aged 50 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with true
		And I answer the question 'Do you exercise more than 1 hour per week' with true
		When I submit the questionnaire
		Then I should see the risk score 4
		And display the message "We think there are some simple things you could do to improve you quality of life, please phone to book an appointment"
  
  Scenario: Calculate risk score with all false inputs and age bracket 41-65
		Given I patient aged 50 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with false
		And I answer the question 'Do you smoke' with false
		And I answer the question 'Do you exercise more than 1 hour per week' with false
		When I submit the questionnaire
		Then I should see the risk score 2
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with mixed inputs and age bracket 41-65
		Given I patient aged 50 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with false
		And I answer the question 'Do you exercise more than 1 hour per week' with true
		When I submit the questionnaire
		Then I should see the risk score 1
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with all medically negative inputs and age bracket 65+
		Given I patient aged 70 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with true
		And I answer the question 'Do you exercise more than 1 hour per week' with false
		When I submit the questionnaire
		Then I should see the risk score 9
		And display the message "We think there are some simple things you could do to improve you quality of life, please phone to book an appointment"

  Scenario: Calculate risk score with all true inputs and age bracket 65+
		Given I patient aged 70 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with true
		And I answer the question 'Do you exercise more than 1 hour per week' with true
		When I submit the questionnaire
		Then I should see the risk score 6
		And display the message "We think there are some simple things you could do to improve you quality of life, please phone to book an appointment"

  Scenario: Calculate risk score with all false inputs and age bracket 65+
		Given I patient aged 70 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with false
		And I answer the question 'Do you smoke' with false
		And I answer the question 'Do you exercise more than 1 hour per week' with false
		When I submit the questionnaire
		Then I should see the risk score 3
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"

  Scenario: Calculate risk score with mixed inputs and age bracket 65+
		Given I patient aged 70 starts the questionnaire
		And I answer the question 'Do you drink on more than 2 days a week' with true
		And I answer the question 'Do you smoke' with false
		And I answer the question 'Do you exercise more than 1 hour per week' with true
		When I submit the questionnaire
		Then I should see the risk score 3
		And display the message "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!"