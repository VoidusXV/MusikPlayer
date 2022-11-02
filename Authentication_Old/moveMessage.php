<?php
    
if(isset($_POST['Username'], $_POST['UsernameID']))
{
    include("FTP_config.php");
    $username = $_POST['Username'];
    $usernameID = $_POST['UsernameID'];
    $TargetClient = $username . "#". $usernameID;
    
    $file_list = ftp_nlist($ftp_conn, "/public_html/Musicedy/Clients/". $TargetClient . "/Unread_Messages/");
    $filesCount =  count(ftp_nlist($ftp_conn, 'public_html/Musicedy/Clients/'. $TargetClient . "/Unread_Messages/")) - 2;
    $MoveTo = "/public_html/Musicedy/Clients/". $TargetClient . "/Messages/";
    //echo "$TargetClient<br>";

    if($filesCount > 0)
    {
        foreach ($file_list as $file)
        {
            $info = pathinfo($file);
            $file_name =  basename($file,'.'.$info['extension']);
            echo $file_name ."\n";
            if(ftp_rename($ftp_conn, $file, $MoveTo . "/". $file_name))
            {
                echo "Successfully uploaded";
            }
            else
            {
                echo "Nope\n";
            }
        }
    }
}
else
{
  echo 'Authentication Error';
}
?>