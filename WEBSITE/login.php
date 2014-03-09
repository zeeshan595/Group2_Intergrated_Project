<?php

if (isset($_COOKIE['Username']))
	header("Location: http://impossiblesix.net");
else if (isset($_POST['submitted']))
{
	require_once("Include/encrypt.php");
	require_once("Include/connect.php");

	$user = stripcslashes(strip_tags(trim($_POST['user'])));
	$pass = stripslashes(strip_tags(trim($_POST['pass'])));
	$pass = urlencode(encrypt($pass));

	$user = mysql_real_escape_string($user);
	$pass = mysql_real_escape_string($pass);

	$query = mysql_query("SELECT * FROM `users` WHERE `Username` = '$user' AND `Password` = '$pass' LIMIT 1");

	if (mysql_num_rows($query) > 0)
	{
		setcookie("Username", urlencode(encrypt($user)), time() + 31536000);
		header("Location: http://impossiblesix.net");
	}
	else
	{
		$message = "Username and password does not match";
	}
}

require_once("Include/header.php");
?>

<div class="main" style="margin-right: 45px;">
	<div class="right" style="margin-left: 85px;">
		<?php if (isset($message)) echo $message; ?>
		<form action="" method="POST">
			<table>
				<tr>
					<td>Username:</td>
					<td><input type="text" name="user" /></td>
				</tr>
				<tr>
					<td>Password:</td>
					<td><input type="password" name="pass" /></td>
				</tr>
			</table>
			<br />
			<input type="submit" name="submitted" value="Login" class="button" />
		</form>
	</div>
</div>

<?php
require_once("Include/footer.php");
?>