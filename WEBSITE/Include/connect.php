<?php
$con = mysql_connect("localhost","root" , "flashframe") or die ("Cannot connect!");
if (!$con)
	die('Could not connect: ' . mysql_error());
$conn = mysql_select_db("ImpossibleSix" , $con) or die ("could not load the database");
?>