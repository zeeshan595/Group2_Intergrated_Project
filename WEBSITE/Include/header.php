<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<link rel="stylesheet" type="text/css" href="css/style.css" />
<title>Impossible Six</title>
<meta name="title" content="Impossible Six">
<meta name="description" content="Awesome Website" />
<meta name="keywords" content="Group 2, Project, Integrated, Project" />
<meta name="author" content="Zeeshan Abid, Kati Salminen" />

<meta name="robots" content="noindex" />
<link rel="shortcut icon" href="Images/favicon.ico" />
</head>

<body>
<div class="content">
<img src="Images/logo.png" class="logo" />

<div class="ribbon">Game Progress Blog</div>
<div class="navBar">
<a href="index.php"><div class="navButton">Home</div></a>
<a href="about.php"><div class="navButton">About</div></a>
<a href="contact.php"><div class="navButton">Contact</div></a>

<?php
if (!isset($_COOKIE['Username']))
	echo "<a href='login.php'><div class='navButton'>Login</div></a>";
else
	echo "<a href='logout.php'><div class='navButton'>Logout</div></a>";
?>

</div>