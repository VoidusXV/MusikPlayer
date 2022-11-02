<?php


if(isset($_POST['Username'], $_POST['UsernameID'] , $_POST['fileName']))
{   
    include("FTP_config.php");
    $username = $_POST['Username'];
    $usernameID = $_POST['UsernameID'];
    $TargetClient = $username . "#". $usernameID;


    $fileCount = count(ftp_nlist($ftp_conn, "/public_html/Musicedy/Clients/". $TargetClient . "/Messages/"));
    $files = ftp_nlist($ftp_conn, "/public_html/Musicedy/Clients/". $TargetClient . "/Messages/");
    $MessageID = $_POST['MessageID'];
    $fileName = $_POST['fileName'];

    if(isset($_POST['Accept_FriendRequest']))
    {
        $TypeOfInvitation = explode("_", $fileName)[0];
        if($TypeOfInvitation == "FriendRequest")
        {
            $messagedClient = explode("_", $fileName)[1];
            //$moveToBlocklist =  "/public_html/Musicedy/Clients/". $TargetClient . "/Friends/$messagedClient";
            //ftp_rename($ftp_conn,  "/public_html/Musicedy/Clients/". $TargetClient . "/Messages/$fileName", $moveToBlocklist);


            ftp_delete($ftp_conn, "/public_html/Musicedy/Clients/". $TargetClient . "/Messages/$fileName"); 
            $file = fopen("../Clients/$TargetClient/Friends/$messagedClient", "w") or die("Unable to open file!");
            fwrite($file, "");// 

            $file = fopen("../Clients/$messagedClient/Friends/$TargetClient", "w") or die("Unable to open file!");
            fwrite($file, ""); //

        }
    }
    else if(isset($_POST["Decline_FriendRequest"]))
    {   
        ftp_delete($ftp_conn, "/public_html/Musicedy/Clients/". $TargetClient . "/Messages/$fileName"); 
    }
    else if(isset($_POST["Block_FriendRequest"]))
    {
        $messagedClient = explode("_", $fileName)[1];
        $moveToBlocklist =  "/public_html/Musicedy/Clients/". $TargetClient . "/Blocklist/$messagedClient";
        ftp_rename($ftp_conn,  "/public_html/Musicedy/Clients/". $TargetClient . "/Messages/$fileName", $moveToBlocklist);
    }
    ftp_close($ftp_conn);
}
else
{
  echo 'Authentication Error';
}

?>