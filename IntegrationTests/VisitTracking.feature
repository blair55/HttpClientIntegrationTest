﻿Feature: Visit Tracking
	In order to know which users have previously visited
	I want to drop a client cookie on each visit

Scenario: Setting client cookie for a new visit
	Given the api uri is local.trackmyvisit.com/api/trackvisit
	When I hit the visit tracking uri
	Then the response HttpCode is OK
	And the response sets a cookie
	And the cookie name is VisitId
	And the value for VisitId is a valid Guid
	And the cookie expiry is 7 days from now