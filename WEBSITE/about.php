<?php
	require_once("Include/header.php");
	require_once("Include/connect.php");
	require_once("Include/encrypt.php");

	$query = mysql_query("SELECT * FROM `users`");
	while($row = mysql_fetch_array($query))
	{
?>
		<div class="main">
			<div class="left"><img src="http://impossiblesix.net/Images/People/<?php echo $row['Username']; ?>.png" /></div>
			<div class="right">Job: <?php echo $row['Job']; ?><br /><br /><?php echo $row['Description']; ?></div>
		</div>
<?php
	}
	require_once("Include/footer.php");
?>