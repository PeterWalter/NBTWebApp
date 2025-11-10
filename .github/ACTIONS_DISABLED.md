Repository Actions Disabled Note

Purpose
-------
This document explains how to disable GitHub Actions for this repository.

Who should disable Actions
--------------------------
Only repository administrators or members of the DevOps/Platform team should disable Actions. Disabling Actions will stop all CI/CD workflows from running for this repository and may interrupt automated deployments or checks.

How to disable Actions (recommended: GitHub web UI)
--------------------------------------------------
1. Open the repository on GitHub.
2. Go to Settings ? Actions ? General.
3. Under "Actions permissions" choose "Disable Actions for this repository".
4. Save changes.

How to disable Actions (REST API)
---------------------------------
You can also disable Actions programmatically using the GitHub REST API. You must use a token with admin/repo scope.

Example (replace OWNER and REPO and set GITHUB_TOKEN):

curl -X PUT \
 -H "Accept: application/vnd.github+json" \
 -H "Authorization: Bearer $GITHUB_TOKEN" \
 https://api.github.com/repos/OWNER/REPO/actions/permissions \
 -d '{"enabled":false}'

Notes and alternatives
----------------------
- If you only need to stop workflows temporarily, consider disabling specific workflow files or renaming/removing files under `.github/workflows/` instead of disabling all Actions.
- Disabling Actions may affect automated checks required by branch protection rules.
- If you are not a repo admin, contact a repository owner or the DevOps team to perform this action.

Contact
-------
If you are unsure whether to disable Actions, contact the DevOps team or repository maintainers before proceeding.
