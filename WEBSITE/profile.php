<?php
if (!isset($_COOKIE['Username']))
	header("Location: http://impossiblesix.net");

require_once("Include/connect.php");
require_once("Include/encrypt.php");

$user = decrypt(urldecode($_COOKIE['Username']));

$job = "";
$description = "";

if (isset($_POST['submitted']))
{
	$job = stripslashes(strip_tags(trim($_POST['job'])));
	$description = stripslashes(strip_tags(trim($_POST['description'])));

	mysql_query("UPDATE `users` SET `Job` = '$job', `Description` = '$description' WHERE `Username` = '$user' LIMIT 1");
}
else
{
	$query = mysql_query("SELECT * FROM `users` WHERE `Username` = '$user' LIMIT 1");
	$data = mysql_fetch_array($query);
	$job = $data['Job'];
	$description = $data['Description'];
}

require_once("Include/header.php");
?>
	<div class="main" style="margin-right: 45px;">
		<div class="right" style="margin-left: 85px;">
			<a href="logout" class="button">Logout</a>
			<a href="password" class="button">Change Password</a>
			<br /><br />
			<form action="" method="POST">
				Job:<br />
				<input type="text" name="job" value="<?php echo $job; ?>" /><br />
				Description:<br />
				<textarea name="description" style="width:100%;height:200px;resize:none;"><?php echo $description; ?></textarea>
				<br /><br />
				<input type="submit" value="save" class="button" name="submitted" />
			</form>
		</div>
	</div>
<?php
require_once("Include/footer.php");
?>