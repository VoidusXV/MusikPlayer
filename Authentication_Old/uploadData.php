<?php
    
if(isset($_POST['Username'], $_POST['UsernameID'], $_POST['fileName']))
{
    include("FTP_config.php");
    $username = $_POST['Username'];
    $usernameID = $_POST['UsernameID'];
    $TargetClient = $username . "#". $usernameID;

    $content = $_POST['content'];
    $fileName = $_POST['fileName'];

    $myfile = fopen("../Clients/$TargetClient/Data/$fileName", "w");
    fwrite($myfile, $content);
    fclose($myfile); 
    ftp_close($ftp_conn);
}
else if (isset($_FILES['file']['tmp_name'], $_POST['FileUploading'], $_POST['Username'], $_POST['UsernameID'])) 
{
    echo "Uploading...\n";
    $username = $_POST['Username'];
    $usernameID = $_POST['UsernameID'];
    $TargetClient = $username . "#". $usernameID;
    if(move_uploaded_file($_FILES["file"]["tmp_name"], "../Clients/$TargetClient/Data/". $_FILES["file"]["name"]))
    {
      echo "Uploaded";
    }
}
else
{
  echo 'Authentication Error';
}
?>