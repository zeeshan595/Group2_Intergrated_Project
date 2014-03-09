<?php
	require_once("Include/header.php");
	require_once("Include/connect.php");
	require_once("Include/encrypt.php");

	if (isset($_COOKIE['Username']))
	{
		if (isset($_POST['submitted']))
		{
			if ($_POST['message'] != "")
			{
				$message = addslashes($_POST['message']);
				$message = mysql_real_escape_string($message);
				mysql_query("INSERT INTO `posts` (`id`, `Message`, `Author`)
					VALUES ('', '$message', '".decrypt(urldecode($_COOKIE['Username']))."')");
			}
			else
			{
				$error = "You can't submit an empty post.";
			}
		}
?>
	<div class="main">
		<div class="left"><img src="http://impossiblesix.net/Images/People/<?php echo decrypt(urldecode($_COOKIE['Username'])); ?>.png" /></div>
		<div class="right">
			<h5>You can use html in the post.<br /><?php if (isset($error)) echo $error; ?></h5>
			<form action="" method="POST">
				<table>
					<tr>
						<td>
							<textarea name="message" style="width:510px;height:200px;resize:none;"></textarea>
						</td>
					</tr>
					<tr>
						<td>
							<input name="submitted" type="submit" class="button" value="Post" />
						</td>
					</tr>
				</table>
			</form>
		</div>
	</div>	
<?php
	}
	$query = mysql_query("SELECT * FROM `posts` ORDER BY `id` DESC LIMIT 30");
	while($row = mysql_fetch_array($query))
	{
?>
		<div class="main">
			<div class="left"><img src="http://impossiblesix.net/Images/People/<?php echo $row['Author']; ?>.png" /></div>
			<div class="right"><?php echo stripcslashes($row['Message']); ?></div>
		</div>
<?php
	}
	require_once("Include/footer.php");
?>