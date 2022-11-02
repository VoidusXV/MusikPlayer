<?php
    
if(isset($_POST['Username'], $_POST['UsernameID']))
{
    include("FTP_config.php");
    $username = $_POST['Username'];
    $usernameID = $_POST['UsernameID'];
    $TargetClient = $username . "#". $usernameID;

    if(isset($_POST['Count']))
    { 
        echo count(ftp_nlist($ftp_conn, 'public_html/Musicedy/Clients/'. $TargetClient . "/Unread_Messages/")) - 2;
        
    }
    else if(isset($_POST['ReadFilesInput']))
    {   
        $file_list = ftp_nlist($ftp_conn, "/public_html/Musicedy/Clients/". $TargetClient . "/Messages/");
        $filesCount =  count(ftp_nlist($ftp_conn, 'public_html/Musicedy/Clients/'. $TargetClient . "/Messages/")) - 2;
        $open = "";
        if($filesCount > 0)
        {     
            
            foreach ($file_list as $file)
            {    
                $info = pathinfo($file);
                $file_name =  basename($file,'.'.$info['extension']);
                if(strpos($file_name, '.') === false)
                {           
                    $open .= file_get_contents("../Clients/$TargetClient/Messages/$file_name");
                }
            }
            echo $open;
        }
        else
        {
            echo "No messages";
        }
    }
    else if(isset($_POST['ShowFriends']))
    {
        $file_list = ftp_nlist($ftp_conn, "/public_html/Musicedy/Clients/". $TargetClient . "/Friends/");
        $filesCount =  count(ftp_nlist($ftp_conn, 'public_html/Musicedy/Clients/'. $TargetClient . "/Friends/")) - 2;

        if($filesCount <=0)
        {
            echo "No Friends";
            return;
        }
        foreach ($file_list as $file)
        {    
            $info = pathinfo($file);
            $friendNames =  basename($file,'.'.$info['extension']);
            if(strpos($friendNames, '.') === false)
            {           
               echo "$friendNames\n";
            }
        }

    }
    else if(isset($_POST['GetFriendData']))
    {
        $file_list = ftp_nlist($ftp_conn, "/public_html/Musicedy/Clients/". $TargetClient . "/Friends/");
        $filesCount =  count(ftp_nlist($ftp_conn, 'public_html/Musicedy/Clients/'. $TargetClient . "/Friends/")) - 2;
        $friendNames_array = array();
        $friendsDataJson_array = array();

        if($filesCount <=0)
        {
            echo "";
            return;
        }
        foreach ($file_list as $file)
        {    
            $info = pathinfo($file);
            $friendNames =  basename($file,'.'.$info['extension']);
            if(strpos($friendNames, '.') === false)
            {           
               array_push($friendNames_array, $friendNames);
            }
        }
        foreach ($friendNames_array as $friend)
        {
            $DataPath = file_get_contents("../Clients/$friend/Data/data.json");
            //array_push($friendsDataJson_array, $DataPath);
            //$json_a = json_decode($DataPath, true);
            print_r($DataPath);
            //echo $json_a;
        }
        //echo count($friendNames_array);
    }
    ftp_close($ftp_conn);
}

else
{
  echo 'Authentication Error';
}
?>