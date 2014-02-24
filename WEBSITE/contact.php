<?php require_once("Include/header.php"); ?>

<div class="page">
	<h2>Contact</h2>
	<table>
		<tr>
			<td>
				Name:
			</td>
			<td>
				<input type="text" name="name" />
			</td>
		</tr>
		<tr>
			<td>
				Subject:
			</td>
			<td>
				<input type="text" name="subject" />
			</td>
		</tr>
	</table>
	<textarea style="width: 630px;height:172px;resize:none;"></textarea>
	<br />
	<input type="submit" value="Send" class="button" />
</div>

<?php require_once("Include/footer.php"); ?>