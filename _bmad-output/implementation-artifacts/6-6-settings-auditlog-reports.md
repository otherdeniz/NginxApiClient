# Story 6.6: Settings, Audit Log & Reports Clients

Status: review

## Story

As a developer,
I want to retrieve/update settings, view audit logs, and access host reports,
so that I have complete visibility into my NPM instance.

## Acceptance Criteria

1. `client.Settings.GetAsync()` and `client.Settings.UpdateAsync(request)` work
2. `client.AuditLog.ListAsync()` returns audit log entries
3. `client.Reports.GetHostsAsync()` returns host statistics
4. Unit tests cover all three clients

## Tasks / Subtasks

- [x] Task 1: Create settings models in `Models/Settings/`
  - [x] `SettingsResponse`, `UpdateSettingsRequest`
- [x] Task 2: Implement SettingsClient
  - [x] Endpoints: `/api/settings`
  - [x] Get and Update operations only
- [x] Task 3: Create audit log models in `Models/AuditLog/`
  - [x] `AuditLogEntry` response model
- [x] Task 4: Implement AuditLogClient
  - [x] Endpoint: `/api/audit-log`
  - [x] List operation only (read-only)
- [x] Task 5: Create reports models in `Models/Reports/`
  - [x] `HostReportResponse` model
- [x] Task 6: Implement ReportsClient
  - [x] Endpoint: `/api/reports/hosts`
  - [x] GetHosts operation only (read-only)
- [x] Task 7: Wire all three into NginxProxyManagerClient root
- [x] Task 8: Write unit tests for all three clients (AC: #4)

## Dev Notes

- **Three simple clients** bundled in one story since each has minimal operations
- **Settings endpoint:** `/api/settings` — get/update default site configuration
- **Audit log endpoint:** `/api/audit-log` — read-only list
- **Reports endpoint:** `/api/reports/hosts` — read-only statistics

### References

- [Source: product-brief-distillate.md#Endpoint Map — Settings, Audit Log, Reports]
- [Source: prd.md#FR32, FR33, FR34]

## Dev Agent Record

### Agent Model Used

Claude Opus 4.6 (1M context)

### Debug Log References

### Completion Notes List

- Implemented SettingsClient (get/update), AuditLogClient (list, read-only), and ReportsClient (GetHosts, read-only); all three wired into the root client; unit tests for all three clients complete.

### Change Log

- 2026-04-17: Story completed and moved to review status.

### File List

