<?php require_once("Include/header.php"); ?>
<?php
	if (isset($_COOKIE['Username']))
	{
?>
	<div class="page" style="border-radius:35px;">
		<textarea style="resize:none;width: 630px;height: 200px;"></textarea>
		<br />
		<input type="submit" value="Post" name="submit" class="button" />
	</div>
<?php
	}
?>
	<div class="post">
		<table>
			<tr>
			<td valign="top">
				<div class="user">
					<img src="Images/user.png" />
					<br />
					<span>
						12.12.12
					</span>
					<br />
					<span><a href="#">
						Admin
					</a></span>
				</div>
				<div class="triangle"></div>
			</td>
			<td>
				<div class="postContent topPost">
					This scentance is false.
				<br /><br /><br />
				<img src="Images/seperator.png" />
				</div>
			</td>
		</tr>
		</table>
	</div>
		
	<div class="post">
		<table>
			<tr>
			<td valign="top">
				<div class="user">
					<img src="Images/user.png" />
					<br />
					<span>
						12.12.12
					</span>
					<br />
					<span><a href="#">
						Admin
					</a></span>
				</div>
				<div class="triangle"></div>
			</td>
			<td>
				<div class="postContent">
					long message long message long message long messagelong messagelong messagelong messagelong messagelong messagelong message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message long message. 
					<br /><br /><br />
					<img src="Images/seperator.png" />
				</div>
			</td>
		</tr>
		</table>
	</div>

	<div class="post">
		<table>
			<tr>
			<td valign="top">
				<div class="user">
					<img src="Images/user.png" />
					<br />
					<span>
						12.12.12
					</span>
					<br />
					<span><a href="#">
						Admin
					</a></span>
				</div>
				<div class="triangle"></div>
			</td>
			<td>
				<div class="postContent bottomPost">
					Link: <a target="_blank" title="Compliments - Band of horses" href="http://www.youtube.com/watch?v=Xw8b0luy6p0">http://www.youtube.com/watch?v=Xw8b0luy6p0</a>
				</div>
			</td>
		</tr>
		</table>
	</div>

<?php require_once("Include/footer.php"); ?>