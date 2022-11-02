<?php

if(isset($_POST['Username'], $_POST['UsernameID'],$_POST['TargetUsername'], $_POST['TargetUsernameID'], $_POST['Type']))
{  
    include("FTP_config.php");

    $username = $_POST['Username'];
    $usernameID = $_POST['UsernameID'];
    $Client = $username . "#". $usernameID;

    $targetUsername = $_POST['TargetUsername'];
    $targetUsernameID = $_POST['TargetUsernameID'];
    $TargetClient = $targetUsername . "#". $targetUsernameID;

    $type = $_POST['Type'];
    $jsonInput = array("Username"=>$username, "UsernameID"=>$usernameID, "Type"=>$type);

    if($type == "Friend_Request")
    {    
        if (file_exists('../Clients/'. $TargetClient)) 
        {                 
            //echo "$Client";   
            //echo "$TargetClient";

            if (file_exists("../Clients/$TargetClient/Blocklist/$Client"))
            {
                echo "Blocked";
                return;
            }
            else if (file_exists("../Clients/$Client/Friends/$TargetClient"))
            {
                echo "Already Friends";
                return;
            }
            $file = fopen('../Clients/'. $TargetClient . "/Unread_Messages/FriendRequest_$Client", "w") or die("Unable to open file!");
            $txt =  json_encode($jsonInput);
            fwrite($file, $txt);
            echo "Friend_Request sent";
                   
        }
        else
        {
            echo "User doesnt exist";
        }      
    }
    else if($type == "Session_Invitation")
    {   
        if (file_exists('../Clients/'. $username. "#". $usernameID)) 
        {  
            $file = fopen('../Clients/'. $TargetClient . "/Unread_Messages/Session_Invitation", "w") or die("Unable to open file!");
            $txt =  json_encode($jsonInput);
            fwrite($file, $txt);
        }
        else
        {
            echo "User doesnt exist";
        }
    }

    ftp_close($ftp_conn);

}
else
{
  echo 'Authentication Error';
}
?>