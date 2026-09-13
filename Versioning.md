# Mile.DotNet.Helpers Versioning

This document applies to all versions of Mile.DotNet.Helpers.

## Version Format

- Simple Version: `<Major>.<Minor> <Tag>`
  - Example: `9.0 Preview 1`
- Binary Version: `<Major>.<Minor>.<Build>.<Revision>`
  - Example: `9.0.2654.0`

## The rule for build and revision number

The build number is the number of days since July 5, 2026 because the first
version of Mile.DotNet.Helpers is created on that day.

The revision number is the number of releases releases in the day corresponding
to the build number, and it counts from zero. So the first revision is 0 and 
the second revision is 1.
