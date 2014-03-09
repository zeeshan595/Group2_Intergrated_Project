<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<title>Impossible Six</title>
		<link rel="stylesheet" href="style.css" type="text/css" />
	</head>
<body>
	
<div id="wrapper">
<div id="header">&nbsp;</div>
<div id="subheader">Game Progress Blog</div>

<div id="navi">
<a href="index" class="navi">Home</a>
<a href="about" class="navi">About</a>
<?php
if (isset($_COOKIE['Username']))
{
?>
	<a href="contact" class="navi">Messages</a>
	<a href="profile" class="navi">Profile</a>
<?php
}
else
{
?>
	<a href="contact" class="navi">Contact</a>
	<a href="login" class="navi">Login</a>
<?php
}
?>
</div>