<?php
	require_once("Include/header.php");
	require_once("Include/connect.php");

	if (isset($_POST['body']))
	{
		if ($_POST['name'] != "" && $_POST['email'] != "" && $_POST['body'] != "")
		{
			$name = $_POST['name'];
			$email = $_POST['email'];
			$body = addslashes($_POST['body']);

			if (!preg_match("/[a-zA-Z0-9\ ]{3,45}/", $name))
			{
				$message = "Your name must contain letters, numbers and a space. It must be between 3 to 45 characters.";
			}
			else if (!preg_match("/[a-zA-Z0-9\_\-\.\%\!]{3,45}@[a-zA-Z0-9]{3,45}.[a-zA-Z]{2,4}/", $email))
			{
				$message = "Your email is not in the correct format.";
			}
			else if (preg_match("/<[^<]+?>/", $body))
			{
				$message = "The message cannot contain any html tags.";
			}
			else
			{
				$body = mysql_real_escape_string($body);
				mysql_query("INSERT INTO `contact` (`id`, `Name`, `Email`, `Message`) VALUES ('', '$name', '$email', '$body')");
				$message = "Your message has been sent, Thank you for contacting us.";
				
				$body = "";
				$name = "";
				$email = "";
			}
		}
		else
		{
			$message = "Please fill in all the fields.";
		}
	}

	if (isset($_COOKIE['Username']))
	{
?>
		<div class="main" style="margin-right: 45px;">
			<div class="right" style="margin-left: 85px;">
				<div style="width:100%;height:500px;overflow-y:scroll;">
<?php
					$query = mysql_query("SELECT * FROM `contact` ORDER BY `id` DESC");
					while($row = mysql_fetch_array($query))
					{
?>
						<a href="message?m=<?php echo $row['id']; ?>">
						<div class="messageBlock">
							<table>
								<tr>
									<td width="100px"><?php echo $row['Name']; ?></td>
									<td><?php if(strlen($row['Message']) < 70) echo $row['Message']; else echo substr($row['Message'], 0, 70) . "..."; ?></td>
								</tr>
							</table>
						</div>
						</a>
<?php
					}
?>
				</div>
			</div>
		</div>
<?php
	}
	else
	{
?>
		<div class="main" style="margin-right: 45px;">
			<div class="right" style="margin-left: 85px;">
				<?php if (isset($message)) echo $message; ?>
				<form action="" method="POST">
					Name:<br />
					<input type="text" name="name" value="<?php if (isset($name)) echo $name; ?>" /><br />
					Email:<br />
					<input type="text" name="email" value="<?php if (isset($email)) echo $email; ?>" /><br />
					Message:<br />
					<textarea name="body" style="width:100%;height:200px;resize:none;"><?php if (isset($body)) echo $body; ?></textarea>
					<br /><br />
					<input type="submit" value="Send" class="button" />
				</form>
			</div>
		</div>
<?php
	}
	require_once("Include/footer.php");
?>