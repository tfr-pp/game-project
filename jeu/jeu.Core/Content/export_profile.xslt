<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="html" indent="yes" />

	<xsl:template match="/PlayerProfile">
		<html>
			<head>
				<title>Profil joueur - <xsl:value-of select="Name" /></title>
				<style>
					body { font-family: Arial, sans-serif; margin: 20px; }
					h1 { color: #333; }
					table { border-collapse: collapse; margin-top: 15px; }
					th, td { border: 1px solid #999; padding: 6px 10px; }
					th { background: #eee; }
				</style>
			</head>
			<body>
				<h1>Profil joueur</h1>

				<p>
					<strong>Nom :</strong>
					<xsl:value-of select="Name" />
				</p>
				<p>
					<strong>Date de création :</strong>
					<xsl:value-of select="CreationDate" />
				</p>
				<p><strong>Musique :</strong> <xsl:value-of select="MusicVolume" />%</p>
				<p><strong>SFX :</strong> <xsl:value-of select="SfxVolume" />%</p>

				<h2>Niveaux</h2>

				<table>
					<tr>
						<th>ID</th>
						<th>Complété</th>
						<th>Temps passé (s)</th>
						<th>Vies restantes</th>
					</tr>

					<xsl:for-each select="Levels/Level">
						<tr>
							<td>
								<xsl:value-of select="@Id" />
							</td>
							<td>
								<xsl:value-of select="@Completed" />
							</td>
							<td>
								<xsl:value-of select="TimeSpent" />
							</td>
							<td>
								<xsl:value-of select="LivesLeft" />
							</td>
						</tr>
					</xsl:for-each>
				</table>

			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>