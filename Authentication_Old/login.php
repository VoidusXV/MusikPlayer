<?php

if(isset($_POST['Email'], $_POST['Password'], $_POST['Last_Login'], $_POST['IP_Address']))
{  
  include("config.php");

  #$username = $_POST['Username'];
  $password = $_POST['Password'];
  $Last_Login = $_POST['Last_Login'];
  $IP_Address = $_POST['IP_Address'];
  $Email = $_POST['Email'];

  #mysqli_real_escape_string($link, $username);
  mysqli_real_escape_string($link, $password);
  mysqli_real_escape_string($link, $Email);

  $sql = "SELECT * FROM Musicedy WHERE Email = ? AND Password = ?";
  $loginCheck = 0;
  
  $UsernameOutput ="";
  $UsernameIDOutput ="";
  $RegisterDateOutput ="";
  if($stmt = mysqli_prepare($link, $sql))
  {
    mysqli_stmt_bind_param($stmt, "ss", $Email,  $password);
    mysqli_stmt_execute($stmt);
    mysqli_stmt_store_result($stmt);    
    $loginCheck =  mysqli_stmt_num_rows($stmt);
  }


  $sql_GetInfo = mysqli_query($link , "SELECT * FROM Musicedy WHERE Email = '".$Email."'And Password = '".$password."'");
  while($row = mysqli_fetch_assoc($sql_GetInfo)) //Get Status
  { 
    $UsernameOutput = $row["Username"];
    $UsernameIDOutput = $row["UsernameID"];
    $RegisterDateOutput = $row["Register_Date"];
  }

  if($loginCheck == 1)
  {
    mysqli_query($link, "UPDATE Musicedy Set IP= '".$IP_Address."',Last_Login= '".$Last_Login."' WHERE Email = '".$Email."'And Password = '".$password."'"); 
    echo "$UsernameOutput\n";
    echo "$UsernameIDOutput\n";
    echo "$RegisterDateOutput\n";
    echo "Login Successful";
  }
  else
  {
    echo 'Email or Password is invalid!';
  }

  mysqli_close($link);
}
else
{
  echo 'Authentication Error';
}





?>
