<?php
if (!isset($_COOKIE['Username']))
	header("Location: http://impossiblesix.net");

require_once("Include/header.php");
require_once("Include/connect.php");
require_once("Include/encrypt.php");

if (isset($_POST['submitted']))
{
	$pass = $_POST['pass'];
	if (preg_match("/[a-zA-Z0-9]+/", $pass))
	{
		if ($pass == $_POST['pass2'])
		{
			$pass = urlencode(encrypt($pass));
			$user = decrypt(urldecode($_COOKIE['Username']));
			mysql_query("UPDATE `users` SET `Password` = '$pass' WHERE `Username` = '$user' LIMIT 1");
			$message = "Password changed.";
		}
		else
		{
			$message = "The passwords do not match.";
		}
	}
	else
	{
		$message = "Your password can only contain letters and numbers.";
	}
}
?>
	<div class="main" style="margin-right: 45px;">
		<div class="right" style="margin-left: 85px;">
			<?php if (isset($message)) echo $message; ?>
			<form action="" method="POST">
				New Password:<br />
				<input type="password" name="pass" /><br />
				Re-New Password:<br />
				<input type="password" name="pass2" />
				<br /><br />
				<input type="submit" name="submitted" value="Change" class="button" />
			</form>
		</div>
	</div>
<?php
require_once("Include/footer.php");
?>