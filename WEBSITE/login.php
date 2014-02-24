<?php

if (isset($_COOKIE['Username']))
	header("Location: index.php");

if (isset($_POST['submit']))
{
	$user = stripcslashes(strip_tags(trim($_POST['user'])));
	$pass = stripcslashes(strip_tags(trim($_POST['pass'])));

	if ($user == "admin" && $pass == "admin")
	{
		setcookie("Username", $user, time() + 3600);
		header("Location: index.php");
	}
}

require_once("Include/header.php");
?>

<div class="page">
	<h2>Login</h2>
	<form action="" method="POST">
	<table>
		<tr>
			<td>Username:
			</td><td>
				<input type="text" name="user" />
			</td>
		</tr>
		<tr>
			<td>Password:
			</td><td>
				<input type="password" name="pass">
			</td>
		</tr>
	</table>
	<br />
	<input type="submit" value="login" class="button" name="submit" />
	</form>
</div>

<?php require_once("Include/footer.php"); ?>