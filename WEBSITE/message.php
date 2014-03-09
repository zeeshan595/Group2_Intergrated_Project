<?php
if (!isset($_COOKIE['Username']))
	header("Location: http://impossiblesix.net");
	
	require_once("Include/header.php");
	require_once("Include/connect.php");

	$m = stripcslashes(strip_tags(trim($_GET['m'])));
	$m = mysql_real_escape_string($m);
	$data = mysql_query("SELECT * FROM `contact` WHERE `id` = '$m' LIMIT 1");
	$data = mysql_fetch_array($data);
	$name = $data["Name"];
	$email = $data["Email"];
	$message = $data["Message"];
?>
	<div class="main" style="margin-right: 45px;">
		<div class="right" style="margin-left: 85px;">
			Name: <strong><?php echo $name; ?></strong><br />
			Email: <strong><?php echo $email; ?></strong><br />
			Message:<br /> <strong><?php echo $message; ?></strong><br />
		</div>
	</div>
<?php
require_once("Include/footer.php");
?>