// ============================================================
// Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGVoteBrickData : brick2x4FData)
{
	category = "CityRPG";
	subCategory = "City Info Bricks";

	uiName = "Vote Brick";

	CityRPGBrickType = $CityBrick_Info;
	CityRPGBrickAdmin = true;
	CityRPGBrickCost = 1000;

	triggerDatablock = CityRPGInputTriggerData;
	triggerSize = "2 4 1";
	trigger = 0;
};

// ============================================================
// Menu
// ============================================================
function CityMenu_Vote(%client, %brick)
{
	if($City::Mayor::Voting == 1)
	{
		%menu = "Apply for Mayor. (Costs: $" @ $Pref::Server::City::Mayor::Cost @ ")"
				TAB "Vote"
				TAB "View candidates"
				TAB "View scores";

		%functions =	"serverCmdRegisterCandidates"
							TAB "CityMenu_Vote_VotePrompt"
							TAB "CityMayor_getCandidates"
							TAB "serverCmdtopC";
	} else {
		if($City::Mayor::ID != -1 && $City::Mayor::ID !$= "") {
			messageClient(%client, '', "\c6City mayor: " @ $City::Mayor::String);

			%menu = "Vote to remove the Mayor from office \c3($" @ $Pref::Server::City::Mayor::ImpeachCost @ ")";

			%functions = "CityMayor_VoteImpeach";
		}

		%client.cityMenuMessage("\c6There currently isn't an election. Check back later.");
	}

	// Open the menu even if there are no options.
	// This way, we can use the cityMenuMessage calls, and the exit message still shows up.
	%client.cityMenuOpen(%menu, %functions, %brick, "\c6Thanks, come again.");
}

function CityMenu_Vote_VotePrompt(%client, %input)
{
	%client.cityMenuMessage("\c6Type the candidate's name you'd like to vote for:");

	%client.cityMenuFunction = "serverCmdvoteElection";
}

// ============================================================
// Trigger Data
// ============================================================
function CityRPGVoteBrickData::parseData(%this, %brick, %client, %triggerStatus, %text)
{
	if(%triggerStatus == true && !%client.cityMenuOpen)
	{
		%client.cityMenuMessage("\c3" @ $Pref::Server::City::name @ " Voting Booth");

		CityMenu_Vote(%client, %brick);
	}
}
