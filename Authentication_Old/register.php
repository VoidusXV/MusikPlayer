<?php

if(isset($_POST['Username'], $_POST['Email'], $_POST['Password'], $_POST['Register_Date'], $_POST['IP_Address']))
{
  include("config.php");
  include("FTP_config.php");
  $username = $_POST['Username'];
  $usernameID = random_int(1, 10000);
  $password = $_POST['Password'];
  $register_date = $_POST['Register_Date'];
  $IP_Address = $_POST['IP_Address'];
  $Email = $_POST['Email'];

  mysqli_real_escape_string($link, $username);
  mysqli_real_escape_string($link, $password);
  mysqli_real_escape_string($link, $Email);
  
  $emailCheckSQL = "SELECT Email FROM Musicedy WHERE Email = ?";
  $emailNums = 0;
  if ($stmt = mysqli_prepare($link, $emailCheckSQL)) 
  {
    mysqli_stmt_bind_param($stmt, "s", $Email);
    mysqli_stmt_execute($stmt);
    mysqli_stmt_store_result($stmt);
    $emailNums = mysqli_stmt_num_rows($stmt);
  }
  
  if($emailNums > 0)
  { 
    echo "Account exists";
    return;
  }
  $UserCount =  mysqli_num_rows(mysqli_query($link , "SELECT * FROM Musicedy"));
  $accountExists = mysqli_query($link , "SELECT UsernameID FROM Musicedy WHERE usernameID = '".$usernameID."'");
  $accountExistCount = mysqli_num_rows($accountExists);

  echo $accountExistCount . "\n";

  if($accountExistCount > 0)
  {
    while($accountExistCount > 0)
    {
      echo "Versuch\n";
      $usernameID = random_int(1, 10000);
      $accountExists = mysqli_query($link , "SELECT UsernameID FROM Musicedy WHERE usernameID = '".$usernameID."'");
      $accountExistCount = mysqli_num_rows($accountExists);
    }
  }
  
  $sql = "INSERT INTO Musicedy (Username, Email, UsernameID, Password, Register_Date, IP) VALUES (?,?, ?, ?, ?, ?)";

  if($stmt = mysqli_prepare($link, $sql))
  {
    mysqli_stmt_bind_param($stmt, "ssssss", $username, $Email, $usernameID, $password, $register_date, $IP_Address);
  }

  if(mysqli_stmt_execute($stmt))
  {
    if (!file_exists('../Clients/'. $username. "#". $usernameID)) 
    {
       mkdir('../Clients/'.$username. "#". $usernameID, 0700, false);
       mkdir('../Clients/'.$username. "#". $usernameID . "/Friends", 0700, false);
       mkdir('../Clients/'.$username. "#". $usernameID . "/Messages", 0700, false);
       mkdir('../Clients/'.$username. "#". $usernameID . "/Unread_Messages", 0700, false);
       mkdir('../Clients/'.$username. "#". $usernameID . "/Playlists", 0700, false);
       mkdir('../Clients/'.$username. "#". $usernameID . "/Session", 0700, false);
       mkdir('../Clients/'.$username. "#". $usernameID . "/Data", 0700, false);
       mkdir('../Clients/'.$username. "#". $usernameID . "/Blocklist", 0700, false);
    }

    echo "created";
  }
  else
  {
    echo "Something went wrong";
  }

  //ftp_close($ftp_conn);
  mysqli_close($link);

}
else
{
  echo 'Authentication Error';
}

?>